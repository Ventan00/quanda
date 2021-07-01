using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Quanda.Client.Repositories.Interfaces;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.DTOs.Responses;
using Quanda.Shared.Enums;

namespace Quanda.Client.Repositories.Implementations
{
    public class UsersRepository : IUsersRepository
    {
        private const string ApiUrl = "/api/accounts";
        private readonly HttpClient _httpClient;
        public UsersRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginStatusEnum> LoginAsync(LoginDTO loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync($"{ApiUrl}/login", loginDto);

            var loginResponseDto = JsonSerializer.Deserialize<LoginResponseDTO>(await response.Content.ReadAsStringAsync(),
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

            return loginResponseDto?.LoginStatus ?? LoginStatusEnum.SERVER_ERROR;
        }
    }
}
