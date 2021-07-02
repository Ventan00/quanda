using Quanda.Server.Utils;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.DTOs.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quanda.Server.Repositories.Interfaces
{
    public interface IAnswerRepository
    {
        Task<List<AnswerBoxResponseDto>> GetAnswersAsync(int idQuestion);

        Task<AnswerResult> AddAnswerAsync(AddAnswerDTO answerDTO);

        Task<AnswerResult> UpdateAnswerAsync(int idAnswer, UpdateAnswerDTO answerDTO);

        Task<AnswerResult> DeleteAnswerAsync(int idAnswer);
    }
}
