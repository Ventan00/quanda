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

            return @registerStatus switch
            {
                USER_REGISTERED => NoContent(),
                USER_EMAIL_IS_TAKEN => Conflict(),
                USER_NICKNAME_IS_TAKEN => Conflict(),
                USER_DB_ERROR => StatusCode((int)HttpStatusCode.InternalServerError),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            var loginResponseDto = new LoginResponseDTO
            {
                LoginStatus = LoginStatusEnum.INVALID_CREDENTIALS
            };

            var user = await _usersRepository.GetUserByEmailAsync(loginDto.Email);
            if (user is null)
                return Unauthorized(loginResponseDto);

            var isPasswordCorrect = _userAuthService.VerifyUserPassword(loginDto.RawPassword, user);
            if (!isPasswordCorrect)
                return Unauthorized(loginResponseDto);

            if (user.IdTempUserNavigation is not null)
            {
                loginResponseDto.LoginStatus = LoginStatusEnum.EMAIL_NOT_CONFIRMED;
                return Unauthorized(loginResponseDto);
            }

            var (refreshToken, refreshTokenExpirationDate) = _jwtService.GenerateRefreshToken();
            var updateStatus = await _usersRepository.UpdateRefreshTokenForUserAsync(user, refreshToken, refreshTokenExpirationDate);
            if (updateStatus == USER_DB_ERROR)
            {
                loginResponseDto.LoginStatus = LoginStatusEnum.SERVER_ERROR;
                return StatusCode((int)HttpStatusCode.InternalServerError, loginResponseDto.LoginStatus == LoginStatusEnum.SERVER_ERROR);
            }

            var accessToken = _jwtService.GenerateAccessToken(user);
            _jwtService.AddTokensToCookies(refreshToken, refreshTokenExpirationDate, accessToken, Response.Cookies);

            loginResponseDto.LoginStatus = LoginStatusEnum.LOGIN_ACCEPTED;
            return Ok(loginResponseDto);
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
    }
}
