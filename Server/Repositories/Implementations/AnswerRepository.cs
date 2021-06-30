using Microsoft.EntityFrameworkCore;
using Quanda.Server.Data;
using Quanda.Server.Repositories.Interfaces;
using Quanda.Server.Utils;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.DTOs.Responses;
using Quanda.Shared.Models;
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

        public async Task<List<AnswerResponseDTO>> GetAnswersAsync(int idQuestion)
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

        public async Task<AnswerResult> AddAnswerAsync(AddAnswerDTO answerDTO)
        {
            var existsQuestion = await _context.Questions.AnyAsync(q => q.IdQuestion == answerDTO.IdQuestion);
            if (!existsQuestion)
                return AnswerResult.QUESTION_DELETED;
            var existsUser = await _context.Users.AnyAsync(u => u.IdUser == answerDTO.IdUser);
            if (!existsUser)
                return AnswerResult.USER_DELETED;
            if(answerDTO.IdRootAnswer != null)
            {
                bool existsRootAnswer = await _context.Answers.AnyAsync(a => a.IdAnswer == answerDTO.IdRootAnswer);
                if (!existsRootAnswer)
                    answerDTO.IdRootAnswer = null;
            }

            await _context.AddAsync(new Answer
            {
                Text = answerDTO.Text,
                IdQuestion = answerDTO.IdQuestion,
                IdUser = answerDTO.IdUser,
                IdRootAnswer = answerDTO.IdRootAnswer
            }
            );
            if (!(await _context.SaveChangesAsync() > 0))
                return AnswerResult.ADD_DB_ERROR;

            return AnswerResult.SUCCESS;
        }

        public async Task<AnswerResult> UpdateAnswerAsync(int idAnswer, UpdateAnswerDTO answerDTO)
        {
            var answer = await _context.Answers.SingleOrDefaultAsync(a => a.IdAnswer == idAnswer);
            if (answer == null)
                return AnswerResult.ANSWER_DELETED;
            answer.Text = answerDTO.Text;
            answer.IsModified = true;
            if (!(await _context.SaveChangesAsync() > 0))
                return AnswerResult.UPDATE_DB_ERROR;
            return AnswerResult.SUCCESS;
        }

        public async Task<AnswerResult> DeleteAnswerAsync(int idAnswer)
        {
            var answer = await _context.Answers.SingleOrDefaultAsync(a => a.IdAnswer == idAnswer);
            if (answer == null)
                return AnswerResult.ANSWER_DELETED;
            _context.Answers.Remove(answer);
            if (!(await _context.SaveChangesAsync() > 0))
                return AnswerResult.DELETE_DB_ERROR;

            return AnswerResult.SUCCESS;
        }
    }
}
