using Microsoft.EntityFrameworkCore;
using Quanda.Server.Data;
using Quanda.Server.Repositories.Interfaces;
using Quanda.Server.Utils;
using Quanda.Shared;
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

        public async Task<List<AnswerResponseDTO>> GetAnswersAsync(int idQuestion, int? idUserLogged, AnswersPageDTO answersParams)
        {
            var answers = await _context.Answers.Where(a => a.IdQuestion == idQuestion && a.IdRootAnswer == null).Select(a => new AnswerResponseDTO
            {
                IdAnswer = a.IdAnswer,
                Text = a.Text,
                Rating = a.RatingAnswers.Select(ra => new { ValueAns = ra.Value == false ? -1 : 1 }).Sum(r => r.ValueAns),
                IsModified = a.IsModified,
                User = new UserResponseDTO
                {
                    IdUser = a.IdUserNavigation.IdUser,
                    Nickname = a.IdUserNavigation.Nickname,
                    Avatar = a.IdUserNavigation.Avatar
                },
                IdRootAnswer = a.IdRootAnswer,
                AnswerChildren = new List<AnswerResponseDTO>(),
                IsLiked = a.RatingAnswers.Any(ra => ra.IdUser == idUserLogged) == false ? 0 : a.RatingAnswers.SingleOrDefault(ra => ra.IdUser == idUserLogged).Value ? 1 : -1,
                AmountOfAnswerChildren = a.InverseIdRootAnswersNavigation.Count,
            }).OrderByDescending(a => a.Rating).ThenBy(a => a.IdAnswer).ToListAsync();
            var pageSize = answersParams.PageSize == 0 ? Config.ANSWERS_PAGE_SIZE : answersParams.PageSize;
            return answers.Skip(answersParams.StartIndex).Take(pageSize).ToList();
        }

        public async Task<List<AnswerResponseDTO>> GetAnswerChildrenAsync(int idAnswer, int? idUserLogged)
        {
            var answerChilds = await _context.Answers.Where(a => a.IdRootAnswer == idAnswer).Select(a => new AnswerResponseDTO
            {
                IdAnswer = a.IdAnswer,
                Text = a.Text,
                Rating = a.RatingAnswers.Select(ra => new { ValueAns = ra.Value == false ? -1 : 1 }).Sum(r => r.ValueAns),
                IsModified = a.IsModified,
                User = new UserResponseDTO
                {
                    IdUser = a.IdUserNavigation.IdUser,
                    Nickname = a.IdUserNavigation.Nickname,
                    Avatar = a.IdUserNavigation.Avatar
                },
                IdRootAnswer = a.IdRootAnswer,
                IsLiked = a.RatingAnswers.Any(ra => ra.IdUser == idUserLogged) == false ? 0 : a.RatingAnswers.SingleOrDefault(ra => ra.IdUser == idUserLogged).Value ? 1 : -1,
            }).OrderByDescending(a => a.Rating).ToListAsync();
            return answerChilds;
        }

        public async Task<AnswerResult> AddAnswerAsync(AddAnswerDTO answerDTO, int? idUserLogged)
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
                IdUser = (int)idUserLogged,
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


        public async Task<AnswerResult> UpdateRatingAnswerAsync(int idAnswer, int? idUserLogged, UpdateRatingAnswerDTO updateRatingAnswer)
        {
            var answerRated = await _context.RatingAnswers.SingleOrDefaultAsync(ra => ra.IdAnswer == idAnswer && ra.IdUser == idUserLogged);
            if (answerRated == null)
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
                    IdUser = (int)idUserLogged,
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
