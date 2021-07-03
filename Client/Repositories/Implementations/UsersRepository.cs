using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Quanda.Client.Helpers;
using Quanda.Client.Repositories.Interfaces;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.DTOs.Responses;
using Quanda.Shared.Enums;

namespace Quanda.Client.Repositories.Implementations
{
    public class UsersRepository : IUsersRepository
    {
        private const string ApiUrl = "/api/accounts";
        private readonly IHttpService _httpService;
        public UsersRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<LoginStatusEnum> LoginAsync(LoginDTO loginDto)
        {
            var response = await _httpService.PostWithResponse<LoginDTO, LoginResponseDTO>($"{ApiUrl}/login", loginDto);
            var loginResponseDto = response.Response;

            return loginResponseDto?.LoginStatus ?? LoginStatusEnum.SERVER_ERROR;
        }

        public async Task<RegisterStatusEnum> RegisterAsync(RegisterDTO registerDto)
        {
            var response = await _httpService.PostWithResponse<RegisterDTO, RegisterResponseDTO>($"{ApiUrl}/register", registerDto);
            var registerResponseDto = response.Response;

            return registerResponseDto?.RegisterStatus ?? RegisterStatusEnum.SERVER_ERROR;
        }

        public async Task ResendConfirmationEmailAsync(RecoverDTO recoverDto)
        {
            await _httpService.Post($"{ApiUrl}/resend-confirmation-email", recoverDto);
        }
    }
}
