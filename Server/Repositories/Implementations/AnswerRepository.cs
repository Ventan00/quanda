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

        public async Task<List<AnswerResponseDTO>> GetAnswersAsync(int idQuestion, int idUserLogged)
        {
            var answers = await _context.Answers.Where(a => a.IdQuestion == idQuestion).Select(a => new AnswerResponseDTO
            {
                IdAnswer = a.IdAnswer,
                Text = a.Text,
                Rating = a.RatingAnswers.Select(ra => new { ValueAns = ra.Value == false ? -1 : 1 }).Sum(r => r.ValueAns),
                IsModified = a.IsModified,
                UserResponseDTO =  new UserResponseDTO { 
                    IdUser = a.IdUserNavigation.IdUser,
                    Nickname = a.IdUserNavigation.Nickname,
                    Avatar = a.IdUserNavigation.Avatar
                },
                IdRootAnswer = a.IdRootAnswer,
                ChildAnswers = new List<AnswerResponseDTO>(),
                Mark = 0
            }).ToListAsync();
            foreach (var ans in answers)
            {
                var ratingAnswer = await _context.RatingAnswers.SingleOrDefaultAsync(ra => ra.IdUser == idUserLogged && ra.IdAnswer == ans.IdAnswer);
                if (ratingAnswer != null)
                    ans.Mark = (ratingAnswer.Value == true ? 1 : -1);
                ans.ChildAnswers = answers.Where(a => a.IdRootAnswer == ans.IdAnswer).ToList();
            }

            return answers.Where(a => a.IdRootAnswer == null).ToList();
        }
        
        public async Task<AnswerResult> AddAnswerAsync(AddAnswerDTO answerDTO, int idUserLogged)
        {
            var existsQuestion = await _context.Questions.AnyAsync(q => q.IdQuestion == answerDTO.IdQuestion);
            if (!existsQuestion)
                return AnswerResult.QUESTION_DELETED;
            var existsUser = await _context.Users.AnyAsync(u => u.IdUser == idUserLogged);
            if (!existsUser)
                return AnswerResult.USER_DELETED;
            if (answerDTO.IdRootAnswer != null)
            {
                bool existsRootAnswer = await _context.Answers.AnyAsync(a => a.IdAnswer == answerDTO.IdRootAnswer);
                if (!existsRootAnswer)
                    answerDTO.IdRootAnswer = null;
            }

            await _context.AddAsync(new Answer
            {
                Text = answerDTO.Text,
                IdQuestion = answerDTO.IdQuestion,
                IdUser = idUserLogged,
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
        
        public async Task<AnswerResult> DeleteAnswerAsync(int idAnswer, int idUserLogged)
        {
            var answer = await _context.Answers.SingleOrDefaultAsync(a => a.IdAnswer == idAnswer);
            if (answer == null)
                return AnswerResult.ANSWER_DELETED;
            _context.Answers.Remove(answer);
            if (answer.IdUser != idUserLogged)
                return AnswerResult.NOT_OWNER_OF_ANSWER;
            if (!(await _context.SaveChangesAsync() > 0))
                return AnswerResult.DELETE_DB_ERROR;

            return AnswerResult.SUCCESS;
        }

        
        public async Task<AnswerResult> UpdateRatingAnswerAsync(int idAnswer, int idUserLogged, UpdateRatingAnswerDTO updateRatingAnswer)
        {
            var answerRated = await _context.RatingAnswers.SingleOrDefaultAsync(ra => ra.IdAnswer == idAnswer && ra.IdUser == idUserLogged);
            if(answerRated == null)
            {
                var existsAnswer = await _context.Answers.AnyAsync(a => a.IdAnswer == idAnswer);
                if (!existsAnswer)
                    return AnswerResult.ANSWER_DELETED;
                var existsUser = await _context.Users.AnyAsync(u => u.IdUser == idUserLogged);
                if (!existsUser)
                    return AnswerResult.USER_DELETED;
                var ownerAnswer = await _context.Answers.AnyAsync(a => a.IdAnswer == idAnswer && a.IdUser == idUserLogged);
                if (ownerAnswer)
                {
                    return AnswerResult.OWNER_OF_ANSWER;
                }

                await _context.AddAsync(new RatingAnswer
                {
                    IdAnswer = idAnswer,
                    IdUser = idUserLogged,
                    Value = updateRatingAnswer.Rating == 1
                });
                if (!(await _context.SaveChangesAsync() > 0))
                    return AnswerResult.ADD_DB_ERROR;
            }
            else
            {
                var ownerAnswer = await _context.Answers.AnyAsync(a => a.IdAnswer == idAnswer && a.IdUser == idUserLogged);
                if (!ownerAnswer)
                {
                        _context.RatingAnswers.Remove(answerRated);
                        if (!(await _context.SaveChangesAsync() > 0))
                            return AnswerResult.DELETE_DB_ERROR;
                }
                else
                    return AnswerResult.OWNER_OF_ANSWER;

            }
            return AnswerResult.SUCCESS;
        }
    }
}
