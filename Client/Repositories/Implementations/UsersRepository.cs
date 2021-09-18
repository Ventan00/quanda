using System;
using System.Collections.Generic;
using System.Net;
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

        public async Task<LoginResponseDTO> LoginAsync(LoginDTO loginDto)
        {
            var response = await _httpService.PostWithResponse<LoginDTO, LoginResponseDTO>($"{ApiUrl}/login", loginDto);
            var loginResponseDto = response.Response;
            return loginResponseDto;
        }

        public async Task<RegisterStatusEnum> RegisterAsync(RegisterDTO registerDto)
        {
            var response =
                await _httpService.PostWithResponse<RegisterDTO, RegisterResponseDTO>($"{ApiUrl}/register",
                    registerDto);
            var registerResponseDto = response.Response;

            return registerResponseDto?.RegisterStatus ?? RegisterStatusEnum.SERVER_ERROR;
        }

        public async Task<bool> RecoverConfirmationEmailAsync(RecoverDTO recoverDto)
        {
            var response = await _httpService.Post($"{ApiUrl}/recover-confirmation-email", recoverDto);
            return response.Success;
        }

        public async Task<bool> RecoverPasswordAsync(RecoverDTO recoverDto)
        {
            var response = await _httpService.Post($"{ApiUrl}/recover-password", recoverDto);
            return response.Success;
        }

        public async Task<HttpStatusCode> ResetPasswordAsync(PasswordResetDTO passwordResetDto)
        {
            var response = await _httpService.Post($"{ApiUrl}/reset-password", passwordResetDto);
            return response.HttpResponseMessage.StatusCode;
        }

        public async Task<IEnumerable<Top3UserResponseDTO>> GetTop3UsersAsync()
        {
            var response = await _httpService.Get<IEnumerable<Top3UserResponseDTO>>($"{ApiUrl}/top3-users");
            return response.Response;
        }
    }
}