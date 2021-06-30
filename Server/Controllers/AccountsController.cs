using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using static Quanda.Server.Enums.UserStatus;
using Quanda.Server.Repositories.Interfaces;
using Quanda.Server.Services.Interfaces;
using Quanda.Shared.DTOs.Requests;

namespace Quanda.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly IUsersRepository _repository;
        private readonly IJwtService _jwtService;
        private readonly IUserAuthService _userAuthService;

        public AccountsController(IUsersRepository repository, IJwtService jwtService, IUserAuthService userAuthService)
        {
            _repository = repository;
            _jwtService = jwtService;
            _userAuthService = userAuthService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registerDto)
        {
            var registerStatus = await _repository.AddNewUserAsync(registerDto);

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
            var user = await _repository.GetUserByEmailAsync(loginDto.Email);
            if (user is null)
                return Unauthorized();

            var isPasswordCorrect = _userAuthService.VerifyUserPassword(loginDto.RawPassword, user);
            if (!isPasswordCorrect)
                return Unauthorized();

            var (refreshToken, refreshTokenExpirationDate) = _jwtService.GenerateRefreshToken();
            var updateStatus = await _repository.UpdateRefreshTokenForUserAsync(user, refreshToken, refreshTokenExpirationDate);
            if (updateStatus == USER_DB_ERROR)
                return StatusCode((int)HttpStatusCode.InternalServerError);

            var accessToken = _jwtService.GenerateAccessToken(user);

            _jwtService.AddTokensToCookies(refreshToken, refreshTokenExpirationDate, accessToken, Response.Cookies);

            return NoContent();
        }
    }
}
