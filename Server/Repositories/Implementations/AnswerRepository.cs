using Microsoft.EntityFrameworkCore;
using Quanda.Server.Data;
using Quanda.Server.Repositories.Interfaces;
using Quanda.Shared.DTOs.Responses;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quanda.Server.Repositories.Implementations
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly AppDbContext _context;
        public AnswerRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<AnswerResponseDTO>> GetAnwsersAsync(int idQuestion)
        {
            var answers = await _context.Answers.Where(a => a.IdQuestion == idQuestion).Select(a => new AnswerResponseDTO 
            {
                IdAnswer = a.IdAnswer,
                Text = a.Text,
                Rating = a.Rating,
                IsModified = a.IsModified,
                IdUser = a.IdUser,
                IdRootAnswer = a.IdRootAnswer
            }).ToListAsync();
            return answers;
        }
    }
}
