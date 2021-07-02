using Quanda.Client.Helpers;
using Quanda.Client.Repositories.Interfaces;
using Quanda.Shared.DTOs.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quanda.Client.Repositories.Implementations
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly IHttpService httpService;
        private readonly string url = "api/answers";

        public AnswerRepository(IHttpService httpService)
        {
            this.httpService = httpService;
        }

        public async Task<List<AnswerBoxResponseDto>> GetAnswersAsync(int idQuestion)
        {
            return (await httpService.Get<List<AnswerBoxResponseDto>>($"{url}/{idQuestion}")).Response;
        }
    }
}
