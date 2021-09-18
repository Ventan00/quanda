using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quanda.Server.Data;
using Quanda.Server.Repositories.Interfaces;
using Quanda.Server.Utils;
using Quanda.Shared;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.DTOs.Responses;
using Quanda.Shared.Enums;
using Quanda.Shared.Models;

namespace Quanda.Server.Repositories.Implementations
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly AppDbContext _context;
        private readonly int _takeAmount = Config.QUESTIONS_PAGINATION_TAKE_SKIP;

        public QuestionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GetQuestionsDTO> GetQuestions(int skip, SortOptionEnum sortOption,
            List<int>? tags)
        {
            var standardQuestionQuery = _context.Questions.AsQueryable();
            var extraQuestionQuery = _context.Questions.AsQueryable();
            int count;
            if (tags.Count != 0)
            {
                foreach (var tag in tags)
                    standardQuestionQuery = standardQuestionQuery
                        .Where(question => question.QuestionTags
                            .Select(qt => qt.IdTag)
                            .Contains(tag)
                        );

                var parentTags = await _context.Tags
                    .Where(tag => tags.Contains(tag.IdTag) && tag.IdMainTag != null)
                    .Select(tag => tag.IdMainTagNavigation.IdTag).ToListAsync();
                tags.AddRange(parentTags);

                extraQuestionQuery = extraQuestionQuery
                    .Where(question => question.QuestionTags
                        .Select(qt => qt.IdTag)
                        .Any(qt => tags.Contains(qt))
                    )
                    .OrderByDescending(question => question.QuestionTags
                        .Count());
            }

            count = await standardQuestionQuery.CountAsync();
            switch (sortOption)
            {
                case SortOptionEnum.Views:
                    standardQuestionQuery = standardQuestionQuery.OrderByDescending(question => question.Views);
                    extraQuestionQuery = extraQuestionQuery.OrderByDescending(question => question.Views);
                    break;
                case SortOptionEnum.Answers:
                    standardQuestionQuery = standardQuestionQuery.OrderByDescending(question => question.Answers.Count);
                    extraQuestionQuery = extraQuestionQuery.OrderByDescending(question => question.Answers.Count);
                    break;
                case SortOptionEnum.Date:
                    standardQuestionQuery = standardQuestionQuery.OrderByDescending(question => question.PublishDate);
                    extraQuestionQuery = extraQuestionQuery.OrderByDescending(question => question.PublishDate);
                    break;
            }

            standardQuestionQuery = standardQuestionQuery.Skip(skip).Take(_takeAmount);
            extraQuestionQuery = extraQuestionQuery.Skip(skip).Take(_takeAmount);

            var standardQuestions = await standardQuestionQuery.Select(question => new QuestionGetQuestionsDTO
            {
                IdQuestion = question.IdQuestion,
                AnswersCount = question.Answers.Count,
                Avatar = question.IdUserNavigation.Avatar,
                Tags = question.QuestionTags.Select(qt => qt.IdTagNavigation.Name).ToList(),
                Description = question.Description,
                Header = question.Header,
                IdUser = question.IdUser,
                IsFinished = question.IsFinished,
                IsModified = question.IsModified,
                Nickname = question.IdUserNavigation.Nickname,
                Views = question.Views,
                PublishDate = question.PublishDate
            }).ToListAsync();
            return new GetQuestionsDTO
            {
                StandardQuestions = standardQuestions,
                ExtraQuestions = await extraQuestionQuery
                    .Where(question => !standardQuestions
                        .Select(sq => sq.IdQuestion)
                        .Contains(question.IdQuestion)
                    )
                    .Select(question => new QuestionGetQuestionsDTO
                    {
                        IdQuestion = question.IdQuestion,
                        AnswersCount = question.Answers.Count,
                        Avatar = question.IdUserNavigation.Avatar,
                        Tags = question.QuestionTags.Select(qt => qt.IdTagNavigation.Name).ToList(),
                        Description = question.Description,
                        Header = question.Header,
                        IdUser = question.IdUser,
                        IsFinished = question.IsFinished,
                        IsModified = question.IsModified,
                        Nickname = question.IdUserNavigation.Nickname,
                        Views = question.Views,
                        PublishDate = question.PublishDate
                    })
                    .ToListAsync(),
                QuestionsCount = count
            };
        }

        public async Task<QuestionResponseDTO> GetQuestion(int idQuestion, int? idUserLogged)
        {
            QuestionResponseDTO question;
            if (idUserLogged == null)
                question = await _context.Questions.Where(question => question.IdQuestion == idQuestion).Select(q =>
                    new QuestionResponseDTO
                    {
                        Header = q.Header,
                        Description = q.Description,
                        PublishDate = q.PublishDate,
                        Views = q.Views,
                        IsFinished = q.IsFinished,
                        IsModified = q.IsModified,
                        User = new UserResponseDTO
                        {
                            IdUser = q.IdUser,
                            Nickname = q.IdUserNavigation.Nickname,
                            Avatar = q.IdUserNavigation.Avatar
                        },
                        AnswersCount = q.Answers.Count,
                        Tags = q.QuestionTags.Select(qc => new TagResponseDTO
                        {
                            IdTag = qc.IdTagNavigation.IdTag,
                            Name = qc.IdTagNavigation.Name
                        }).ToList(),
                        Answers = q.Answers.Where(a => a.IdRootAnswer == null).Select(a => new AnswerResponseDTO
                        {
                            IdAnswer = 0,
                            Text = "We invite you to create an account.",
                            Rating = a.RatingAnswers.Select(ra => new { ValueAns = ra.Value == false ? -1 : 1 })
                                .Sum(r => r.ValueAns),
                            IsModified = a.IsModified,
                            User = new UserResponseDTO
                            {
                                Nickname = a.IdUserNavigation.Nickname,
                                Avatar = a.IdUserNavigation.Avatar
                            },
                            AnswerChildren = new List<AnswerResponseDTO>(),
                            IsLiked = a.RatingAnswers.Any(ra => ra.IdUser == idUserLogged) == false ? 0 :
                                a.RatingAnswers.SingleOrDefault(ra => ra.IdUser == idUserLogged).Value ? 1 : -1
                        }).OrderByDescending(a => a.Rating).ThenBy(a => a.IdAnswer).ToList()
                    }).SingleOrDefaultAsync();
            else
                question = await _context.Questions.Where(question => question.IdQuestion == idQuestion).Select(q =>
                    new QuestionResponseDTO
                    {
                        IdQuestion = q.IdQuestion,
                        Header = q.Header,
                        Description = q.Description,
                        PublishDate = q.PublishDate,
                        Views = q.Views,
                        IsFinished = q.IsFinished,
                        IsModified = q.IsModified,
                        User = new UserResponseDTO
                        {
                            IdUser = q.IdUser,
                            Nickname = q.IdUserNavigation.Nickname,
                            Avatar = q.IdUserNavigation.Avatar
                        },
                        AnswersCount = q.Answers.Count,
                        Tags = q.QuestionTags.Select(qc => new TagResponseDTO
                        {
                            IdTag = qc.IdTagNavigation.IdTag,
                            Name = qc.IdTagNavigation.Name
                        }).ToList(),
                        Answers = q.Answers.Where(a => a.IdRootAnswer == null).Select(a => new AnswerResponseDTO
                        {
                            IdAnswer = a.IdAnswer,
                            Text = a.Text,
                            Rating = a.RatingAnswers.Select(ra => new { ValueAns = ra.Value == false ? -1 : 1 })
                                .Sum(r => r.ValueAns),
                            IsModified = a.IsModified,
                            User = new UserResponseDTO
                            {
                                IdUser = a.IdUserNavigation.IdUser,
                                Nickname = a.IdUserNavigation.Nickname,
                                Avatar = a.IdUserNavigation.Avatar
                            },
                            IdRootAnswer = a.IdRootAnswer,
                            AnswerChildren = new List<AnswerResponseDTO>(),
                            IsLiked = a.RatingAnswers.Any(ra => ra.IdUser == idUserLogged) == false ? 0 :
                                a.RatingAnswers.SingleOrDefault(ra => ra.IdUser == idUserLogged).Value ? 1 : -1,
                            AmountOfAnswerChildren = a.InverseIdRootAnswersNavigation.Count
                        }).OrderByDescending(a => a.Rating).ThenBy(a => a.IdAnswer).ToList()
                    }).SingleOrDefaultAsync();

            if (question != null)
            {
                question.Answers = question.Answers.Skip(0).Take(idUserLogged != null ? Config.ANSWERS_PAGE_SIZE : 1)
                    .ToList();
                question.Views++;
                var questionUpd = new Question
                {
                    IdQuestion = idQuestion,
                    Views = question.Views
                };
                _context.Questions.Attach(questionUpd);
                _context.Entry(questionUpd).Property(q => q.Views).IsModified = true;
                await _context.SaveChangesAsync();
            }

            return question;
        }

        public async Task<QuestionStatusResult> AddQuestion(AddQuestionDTO question)
        {
            var Question = new Question
            {
                Header = question.Header,
                Description = question.Description,
                PublishDate = DateTime.Now,
                Views = 0,
                IsFinished = false,
                IsModified = false,
                ToCheck = false,
                IdUser = question.IdUser
            };
            await _context.AddAsync(Question);
            return await _context.SaveChangesAsync() == 1
                ? QuestionStatusResult.QUESTION_ADDED
                : QuestionStatusResult.QUESTION_DATABASE_ERROR;
        }

        public async Task<QuestionStatusResult> UpdateQuestion(int questionId, UpdateQuestionDTO question)
        {
            var Question = await _context.Questions.Where(question => question.IdQuestion == questionId)
                .SingleOrDefaultAsync();
            if (Question == null) return QuestionStatusResult.QUESTION_NOT_FOUND;
            Question.Description = question.Description;
            Question.IsModified = true;
            return await _context.SaveChangesAsync() == 1
                ? QuestionStatusResult.QUESTION_UPDATED
                : QuestionStatusResult.QUESTION_DATABASE_ERROR;
        }

        public async Task<QuestionStatusResult> RemoveQuestion(int questionId)
        {
            var Question = await _context.Questions.Where(question => question.IdQuestion == questionId)
                .SingleOrDefaultAsync();
            if (Question == null) return QuestionStatusResult.QUESTION_NOT_FOUND;
            _context.Remove(Question);

            return await _context.SaveChangesAsync() == 1
                ? QuestionStatusResult.QUESTION_DELETED
                : QuestionStatusResult.QUESTION_DATABASE_ERROR;
        }

        public async Task<QuestionStatusResult> SetToCheck(int questionId, bool value)
        {
            var Question = await _context.Questions.Where(question => question.IdQuestion == questionId)
                .SingleOrDefaultAsync();
            if (Question == null) return QuestionStatusResult.QUESTION_NOT_FOUND;
            Question.ToCheck = value;
            ;
            return await _context.SaveChangesAsync() == 1
                ? QuestionStatusResult.QUESTION_CHANGED_TOCHECK_STATUS
                : QuestionStatusResult.QUESTION_DATABASE_ERROR;
        }

        public async Task<QuestionStatusResult> SetFinished(int questionId)
        {
            var Question = await _context.Questions.Where(question => question.IdQuestion == questionId)
                .SingleOrDefaultAsync();
            if (Question == null) return QuestionStatusResult.QUESTION_NOT_FOUND;
            Question.IsFinished = true;
            return await _context.SaveChangesAsync() == 1
                ? QuestionStatusResult.QUESTION_SET_TO_FINISHED
                : QuestionStatusResult.QUESTION_DATABASE_ERROR;
        }

        public async Task<int> GetAmountOfQuestions(List<int> category)
        {
            if (category.Count == 0)
                return await _context.Questions.CountAsync();
            return await _context.QuestionTags
                .Where(qt => category.Any(tag => qt.IdTag == tag))
                .GroupBy(qt => qt.IdQuestion)
                .Select(qcg => qcg.Key)
                .CountAsync();
        }

        public async Task<IEnumerable<QuestionInProfileResponseDto>> GetUserProfileQuestionsAsync(int idUser, int skip)
        {
            return await _context.Questions
                .Where(q => q.IdUser == idUser)
                .OrderByDescending(q => q.PublishDate)
                .Skip(skip)
                .Take(Config.ProfileQuestionsPageSize)
                .Select(q => new QuestionInProfileResponseDto
                {
                    IdQuestion = q.IdQuestion,
                    Header = q.Header,
                    Description = q.Description,
                    Answers = q.Answers.Count,
                    Views = q.Views,
                    CreatedAt = q.PublishDate,
                    Tags = q.QuestionTags.Select(qt => new TagResponseDTO
                    {
                        IdTag = qt.IdTag,
                        Name = qt.IdTagNavigation.Name
                    })
                })
                .ToListAsync();
        }

        public Task<int> GetAmountOfUserQuestionsAsync(int idUser)
        {
            return _context.Questions
                .Where(q => q.IdUser == idUser)
                .CountAsync();
        }
    }
}