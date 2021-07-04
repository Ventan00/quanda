using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static Quanda.Server.Utils.UserStatus;
using static Quanda.Server.Utils.TempUserResult;
using Quanda.Server.Repositories.Interfaces;
using Quanda.Server.Services.Interfaces;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.DTOs.Responses;
using Quanda.Shared.Enums;

namespace Quanda.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IUsersRepository _usersRepository;
        private readonly ITempUsersRepository _tempUsersRepository;
        private readonly IJwtService _jwtService;
        private readonly IUserAuthService _userAuthService;
        private readonly ISmtpService _smtpService;

        public AccountsController(
            IUsersRepository usersRepository,
            ITempUsersRepository tempUsersRepository,
            IJwtService jwtService,
            IUserAuthService userAuthService,
            ISmtpService smtpService)
        {
            _usersRepository = usersRepository;
            _tempUsersRepository = tempUsersRepository;
            _jwtService = jwtService;
            _userAuthService = userAuthService;
            _smtpService = smtpService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            var confirmationCode = Guid.NewGuid().ToString();

            var registerStatus = await _usersRepository.AddNewUserAsync(registerDto, confirmationCode);

            if (registerStatus == USER_REGISTERED)
            {
                await _smtpService.SendRegisterConfirmationEmailAsync(registerDto.Email, confirmationCode);
            }

            var response = new RegisterResponseDTO();

            switch (registerStatus)
            {
                case USER_REGISTERED:
                    response.RegisterStatus = RegisterStatusEnum.REGISTER_FINISHED;
                    return Ok(response);
                case USER_EMAIL_IS_TAKEN:
                    response.RegisterStatus = RegisterStatusEnum.EMAIL_IS_TAKEN;
                    return Conflict(response);
                case USER_NICKNAME_IS_TAKEN:
                    response.RegisterStatus = RegisterStatusEnum.NICKNAME_IS_TAKEN;
                    return Conflict(response);
                case USER_DB_ERROR:
                    response.RegisterStatus = RegisterStatusEnum.SERVER_ERROR;
                    return StatusCode((int)HttpStatusCode.InternalServerError);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var response = new LoginResponseDTO
            {
                LoginStatus = LoginStatusEnum.INVALID_CREDENTIALS
            };

            var user = await _usersRepository.GetUserByEmailAsync(loginDto.Email);
            if (user is null)
                return Unauthorized(response);

            var isPasswordCorrect = _userAuthService.VerifyUserPassword(loginDto.RawPassword, user);
            if (!isPasswordCorrect)
                return Unauthorized(response);

            if (user.IdTempUserNavigation is not null)
            {
                response.LoginStatus = LoginStatusEnum.EMAIL_NOT_CONFIRMED;
                return Unauthorized(response);
            }

            var (refreshToken, refreshTokenExpirationDate) = _jwtService.GenerateRefreshToken();
            var updateStatus = await _usersRepository.UpdateRefreshTokenForUserAsync(user, refreshToken, refreshTokenExpirationDate);
            if (updateStatus == USER_DB_ERROR)
            {
                response.LoginStatus = LoginStatusEnum.SERVER_ERROR;
                return StatusCode((int)HttpStatusCode.InternalServerError, response.LoginStatus == LoginStatusEnum.SERVER_ERROR);
            }

            var accessToken = _jwtService.GenerateAccessToken(user);
            _jwtService.AddTokensToCookies(refreshToken, refreshTokenExpirationDate, accessToken, Response.Cookies);

            response.LoginStatus = LoginStatusEnum.LOGIN_ACCEPTED;
            return Ok(response);
        }


        [HttpGet("confirm-email/{code}")]
        public async Task<IActionResult> ConfirmEmail(string code)
        {
            var deleteResult = await _tempUsersRepository.DeleteTempUserByCodeAsync(code);

            return @deleteResult switch
            {
                TEMP_USER_NOT_FOUND => NotFound("Wrong confirmation code"),
                TEMP_USER_DELETED => Redirect($"https://{HttpContext.Request.Host}/login"),
                TEMP_USER_DB_ERROR => StatusCode((int)HttpStatusCode.InternalServerError),
                _ => throw new ArgumentOutOfRangeException()
            };
        }


        [HttpPost("resend-confirmation-email")]
        public async Task<IActionResult> ResendConfirmationEmail([FromBody] RecoverDTO recoverDto)
        {
            var code = await _tempUsersRepository.GetConfirmationCodeForUserAsync(recoverDto.Email);
            if (code == null)
                return NoContent();

            var isExtended = await _tempUsersRepository.ExtendValidityAsync(recoverDto.Email);
            if (!isExtended)
                return StatusCode((int)HttpStatusCode.InternalServerError);

            await _smtpService.SendRegisterConfirmationEmailAsync(recoverDto.Email, code);

            return NoContent();
        }

        [HttpGet("data/for-question/{idUser:int}")]
        public async Task<IActionResult> GetUserForQuestionByIDDTO([FromRoute]int idUser)
        {
            var result =await _usersRepository.GetUserForQuestionByID(idUser);
            if (result == null)
                return NotFound();
            else
                return Ok(result);
        }
    }
}
