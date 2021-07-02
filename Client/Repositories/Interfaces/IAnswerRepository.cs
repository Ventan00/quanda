using Quanda.Shared.DTOs.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quanda.Client.Repositories.Interfaces
{
    public interface IAnswerRepository
    {
        Task<List<AnswerBoxResponseDto>> GetAnswersAsync(int idQuestion);
    }
}
