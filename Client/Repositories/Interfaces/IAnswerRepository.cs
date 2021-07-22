using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quanda.Client.Repositories.Interfaces
{
    public interface IAnswerRepository
    {
        Task<List<AnswerResponseDTO>> GetAnswersAsync(int idQuestion, AnswersPageDTO productParams);

        Task<Tuple<bool, string>> UpdateRatingAnswerAsync(int idAnswer, int rating);

        Task<Tuple<bool, string>> DeleteAnswer(int idAnswer);

        Task<Tuple<bool, string>> UpdateAnswer(int idAnswer, string text);

        Task<Tuple<bool, string>> AddAnswer(string text, int idQuestion, int idRootAnswer);
    }
}
