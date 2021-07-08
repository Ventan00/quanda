using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    [AllowAnonymous]
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

            response.RefreshToken = refreshToken;
            response.AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken);
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

        [HttpPost("recover-confirmation-email")]
        public async Task<IActionResult> RecoverConfirmationEmail([FromBody] RecoverDTO recoverDto)
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

        [HttpPost("recover-password")]
        public async Task<IActionResult> RecoverPassword([FromBody] RecoverDTO recoverDto)
        {
            var user = await _usersRepository.GetUserByEmailAsync(recoverDto.Email);
            if (user is null || user.IdTempUserNavigation is not null)
                return NoContent();

            var recoveryJwt = _jwtService.GeneratePasswordRecoveryToken(user);
            var base64UrlEncodedRecoveryJwt = Microsoft.IdentityModel.Tokens.Base64UrlEncoder
                .Encode(new JwtSecurityTokenHandler().WriteToken(recoveryJwt));

            await _smtpService.SendPasswordRecoveryEmailAsync(recoverDto.Email, base64UrlEncodedRecoveryJwt, user.IdUser);

            return NoContent();
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetDTO passwordResetDto)
        {
            var providedUser = await _usersRepository.GetUserByIdAsync((int)passwordResetDto.IdUser);
            if (providedUser is null || providedUser.IdTempUserNavigation is not null)
                return BadRequest();

            var decodedRecoveryJwt = Microsoft.IdentityModel.Tokens.Base64UrlEncoder
                .Decode(passwordResetDto.UrlEncodedRecoveryJwt);

            var decryptedIdUser = _jwtService.DecryptPasswordRecoveryToken(decodedRecoveryJwt, providedUser);
            if (decryptedIdUser == null || decryptedIdUser != providedUser.IdUser)
                return BadRequest();

            var isChanged = await _usersRepository.SetNewPasswordForUser(providedUser, passwordResetDto.RawPassword);
            if (!isChanged)
                return StatusCode((int)HttpStatusCode.InternalServerError);

            return NoContent();
        }
    }
}
