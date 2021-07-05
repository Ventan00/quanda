using Quanda.Shared.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quanda.Client.Repositories.Interfaces
{
    public interface IAnswerRepository
    {
        Task<List<AnswerBoxResponseDto>> GetAnswersAsync(int idQuestion);

        Task<Tuple<bool,string>> UpdateRatingAnswerAsync(int idAnswer, int rating);
    }
}
