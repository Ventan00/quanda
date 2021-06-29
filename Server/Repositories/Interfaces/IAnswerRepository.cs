using Quanda.Shared.DTOs.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quanda.Server.Repositories.Interfaces
{
    public interface IAnswerRepository
    {
        Task<List<AnswerResponseDTO>> GetAnwsersAsync(int idQuestion);
    }
}
