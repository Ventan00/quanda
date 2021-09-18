using System;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Quanda.Server.Extensions;
using Quanda.Server.Repositories.Interfaces;
using Quanda.Server.Services.Interfaces;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.DTOs.Responses;
using Quanda.Shared.Enums;
using static Quanda.Server.Utils.UserStatus;
using static Quanda.Server.Utils.TempUserResult;

namespace Quanda.Server.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly ICaptchaService _captchaService;
        private readonly IJwtService _jwtService;
        private readonly ISmtpService _smtpService;
        private readonly ITempUsersRepository _tempUsersRepository;
        private readonly IUserAuthService _userAuthService;
        private readonly IUsersRepository _usersRepository;

        public AccountsController(
            IUsersRepository usersRepository,
            ITempUsersRepository tempUsersRepository,
            IJwtService jwtService,
            IUserAuthService userAuthService,
            ISmtpService smtpService,
            ICaptchaService captchaService)
        {
            _usersRepository = usersRepository;
            _tempUsersRepository = tempUsersRepository;
            _jwtService = jwtService;
            _userAuthService = userAuthService;
            _smtpService = smtpService;
            _captchaService = captchaService;
        }

        /// <summary>
        ///     Rejestracja nowego użytkownika
        /// </summary>
        /// <param name="registerDto"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            var isCaptchaCorrect = await _captchaService.VerifyCaptchaAsync(registerDto.CaptchaResponseToken);
            if (!isCaptchaCorrect)
                return Unauthorized(new RegisterResponseDTO
                {
                    RegisterStatus = RegisterStatusEnum.INVALID_CAPTCHA
                });

            var confirmationCode = Guid.NewGuid().ToString();

            var registerStatus = await _usersRepository.AddNewUserAsync(registerDto, confirmationCode);

            if (registerStatus == USER_REGISTERED)
                await _smtpService.SendRegisterConfirmationEmailAsync(registerDto.Email, confirmationCode);

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

        /// <summary>
        ///     Logowanie użytkownika
        /// </summary>
        /// <param name="loginDto"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            //zamockowany losowy użytkownik do momentu faktycznego działania mechanizmu
            /*return Ok(new LoginResponseDTO()
            {
                RefreshToken = _jwtService.GenerateRefreshToken().refreshToken,
                AccessToken = _jwtService.WriteToken(_jwtService.GenerateAccessToken(new User())),
                Avatar = "https://is2-ssl.mzstatic.com/image/thumb/Purple128/v4/b1/ea/32/b1ea3207-6343-35de-a2c1-3132ff367eea/source/512x512bb.jpg",
                LoginStatus = LoginStatusEnum.LOGIN_ACCEPTED
            });*/
            var isCaptchaCorrect = await _captchaService.VerifyCaptchaAsync(loginDto.CaptchaResponseToken);
            if (!isCaptchaCorrect)
                return Unauthorized(new LoginResponseDTO
                {
                    LoginStatus = LoginStatusEnum.INVALID_CAPTCHA
                });

            var response = new LoginResponseDTO
            {
                LoginStatus = LoginStatusEnum.INVALID_CREDENTIALS
            };

            var user = await _usersRepository.GetUserByNicknameOrEmailAsync(loginDto.NicknameOrEmail);
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
            var updateStatus =
                await _usersRepository.UpdateRefreshTokenForUserAsync(user, refreshToken, refreshTokenExpirationDate);
            if (updateStatus == USER_DB_ERROR)
            {
                response.LoginStatus = LoginStatusEnum.SERVER_ERROR;
                return StatusCode((int)HttpStatusCode.InternalServerError,
                    response.LoginStatus == LoginStatusEnum.SERVER_ERROR);
            }

            var accessToken = _jwtService.GenerateAccessToken(user);

            response.RefreshToken = refreshToken;
            response.AccessToken = _jwtService.WriteToken(accessToken);
            response.Avatar = user.Avatar;
            response.LoginStatus = LoginStatusEnum.LOGIN_ACCEPTED;

            return Ok(response);
        }

        /// <summary>
        ///     Potwierdzenie konta nowo-zarejestrowanego użytkownika poprzez email
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet("confirm-email/{code}")]
        public async Task<IActionResult> ConfirmEmail(string code)
        {
            var deleteResult = await _tempUsersRepository.DeleteTempUserByCodeAsync(code);

            return deleteResult switch
            {
                TEMP_USER_NOT_FOUND => NotFound("Wrong confirmation code"),
                TEMP_USER_DELETED => Redirect($"https://{HttpContext.Request.Host}/login"),
                TEMP_USER_DB_ERROR => StatusCode((int)HttpStatusCode.InternalServerError),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        /// <summary>
        ///     Odzyskanie emailu sluzacego potwierdzeniu konta nowo-zarejestrowanego uzytkownika
        /// </summary>
        /// <param name="recoverDto"></param>
        /// <returns></returns>
        [HttpPost("recover-confirmation-email")]
        public async Task<IActionResult> RecoverConfirmationEmail([FromBody] RecoverDTO recoverDto)
        {
            var isCaptchaCorrect = await _captchaService.VerifyCaptchaAsync(recoverDto.CaptchaResponseToken);
            if (!isCaptchaCorrect) return Unauthorized();

            var code = await _tempUsersRepository.GetConfirmationCodeForUserAsync(recoverDto.Email);
            if (code == null)
                return NoContent();

            var isExtended = await _tempUsersRepository.ExtendValidityAsync(recoverDto.Email);
            if (!isExtended)
                return StatusCode((int)HttpStatusCode.InternalServerError);

            await _smtpService.SendRegisterConfirmationEmailAsync(recoverDto.Email, code);

            return NoContent();
        }

        /// <summary>
        ///     Odzyskanie hasła poprzez wysłany email
        /// </summary>
        /// <param name="recoverDto"></param>
        /// <returns></returns>
        [HttpPost("recover-password")]
        public async Task<IActionResult> RecoverPassword([FromBody] RecoverDTO recoverDto)
        {
            var isCaptchaCorrect = await _captchaService.VerifyCaptchaAsync(recoverDto.CaptchaResponseToken);
            if (!isCaptchaCorrect) return Unauthorized();

            var user = await _usersRepository.GetUserByEmailAsync(recoverDto.Email);
            if (user is null || user.IdTempUserNavigation is not null)
                return NoContent();

            var recoveryJwt = _jwtService.GeneratePasswordRecoveryToken(user);
            var base64UrlEncodedRecoveryJwt = Base64UrlEncoder
                .Encode(_jwtService.WriteToken(recoveryJwt));

            await _smtpService.SendPasswordRecoveryEmailAsync(recoverDto.Email, base64UrlEncodedRecoveryJwt,
                user.IdUser);

            return NoContent();
        }

        /// <summary>
        ///     Ustawienie nowego hasła możliwe po jego odzyskaniu
        /// </summary>
        /// <param name="passwordResetDto"></param>
        /// <returns></returns>
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetDTO passwordResetDto)
        {
            var isCaptchaCorrect = await _captchaService.VerifyCaptchaAsync(passwordResetDto.CaptchaResponseToken);
            if (!isCaptchaCorrect) return Unauthorized();

            var providedUser = await _usersRepository.GetUserByIdAsync((int)passwordResetDto.IdUser);
            if (providedUser is null || providedUser.IdTempUserNavigation is not null)
                return Forbid();

            var decodedRecoveryJwt = Base64UrlEncoder
                .Decode(passwordResetDto.UrlEncodedRecoveryJwt);

            var principal = _jwtService.GetPrincipalFromPasswordRecoveryToken(decodedRecoveryJwt, providedUser);
            if (principal is null)
                return Forbid();

            var claimIdUser = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (claimIdUser is null || !int.TryParse(claimIdUser, out _))
                return Forbid();

            if (providedUser.IdUser != int.Parse(claimIdUser))
                return Forbid();

            var isChanged = await _usersRepository.SetNewPasswordForUser(providedUser, passwordResetDto.RawPassword);
            if (!isChanged)
                return StatusCode((int)HttpStatusCode.InternalServerError);

            return NoContent();
        }

        /// <summary>
        ///     Odświerzenie tokenów zalogowanego użytkownika
        /// </summary>
        /// <param name="refreshDto"></param>
        /// <returns></returns>
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestDTO refreshDto)
        {
            var principal = _jwtService.GetPrincipalFromExpiredToken(refreshDto.AccessToken);
            if (principal is null)
                return BadRequest();

            var claimIdUser = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (claimIdUser is null || !int.TryParse(claimIdUser, out _))
                return BadRequest();

            var user = await _usersRepository.GetUserByRefreshTokenAsync(refreshDto.RefreshToken);
            if (user is null || user.RefreshTokenExpirationDate <= DateTime.UtcNow ||
                user.IdUser != int.Parse(claimIdUser))
                return BadRequest();

            var (refreshToken, expirationDate) = _jwtService.GenerateRefreshToken();
            var updateStatus =
                await _usersRepository.UpdateRefreshTokenForUserAsync(user, refreshToken, expirationDate);
            if (updateStatus != USER_REFRESH_TOKEN_UPDATED)
                return StatusCode((int)HttpStatusCode.InternalServerError);

            var accessToken = _jwtService.GenerateAccessToken(user);

            return Ok(new RefreshResponseDTO
            {
                AccessToken = _jwtService.WriteToken(accessToken),
                RefreshToken = refreshToken
            });
        }

        /// <summary>
        ///     Wylogowanie zalogowanego użytkownika
        /// </summary>
        /// <param name="logoutDto"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutDTO logoutDto)
        {
            var user = await _usersRepository.GetUserByRefreshTokenAsync(logoutDto.RefreshToken);
            if (user == null)
                return NotFound();

            if (user.IdUser != HttpContext.User.GetId())
                return NotFound();

            var updateStatus = await _usersRepository.UpdateRefreshTokenForUserAsync(user, null, null);
            if (updateStatus != USER_REFRESH_TOKEN_UPDATED)
                return StatusCode((int)HttpStatusCode.InternalServerError);

            return NoContent();
        }

        [Authorize]
        [HttpGet("{idUser:int}/profile-details")]
        public async Task<IActionResult> GetUserProfileDetails(
            [FromRoute] int idUser)
        {
            var userDetails = await _usersRepository
                .GetUserProfileDetailsAsync(idUser);

            if (userDetails is null)
                return NotFound();

            var requestIdUser = HttpContext.User.GetId();
            if (requestIdUser != idUser)
                await _usersRepository.AddViewForUserAsync(idUser);

            return Ok(userDetails);
        }
    }
}