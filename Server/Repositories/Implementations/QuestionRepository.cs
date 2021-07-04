using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quanda.Server.Data;
using Quanda.Server.Repositories.Interfaces;
using Quanda.Server.Utils;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.Enums;
using Quanda.Shared.Models;

namespace Quanda.Server.Repositories.Implementations
{
    public class QuestionRepository: IQuestionRepository
    {
        private readonly AppDbContext _context;
        private readonly int _takeAmount = 10;
        public QuestionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Question>> GetQuestions(int skip, SortOptionEnum sortOption, List<int>? categories)
        {
            if (categories.Count() == 0)
            {
                switch (sortOption)
                {
                    case SortOptionEnum.Date:
                        return await _context.Questions
                            .OrderBy(question => question.PublishDate)
                            .Skip(skip)
                            .Take(_takeAmount)
                            .ToListAsync();
                    case SortOptionEnum.Tags:
                        return null;
                    case SortOptionEnum.Views:
                        return await _context.Questions
                            .OrderBy(question => question.Views)
                            .Skip(skip)
                            .Take(_takeAmount)
                            .ToListAsync();
                    case SortOptionEnum.Answers:
                        return await _context.Questions
                            .Include(question => question.Answers)
                            .OrderBy(question => question.Views)
                            .Skip(skip)
                            .Take(_takeAmount)
                            .ToListAsync();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                switch (sortOption)
                {
                    case SortOptionEnum.Date:
                        return await _context.Questions
                            .Include(question => question.QuestionCategories)
                            .Where(
                                question => _context.QuestionCategories
                                    .Include(qc => qc.IdCategoryNavigation.IdCategory)
                                    .Where(qc => qc.IdQuestion==question.IdQuestion)
                                    .Any(qc => categories.Any(cat => qc.IdCategoryNavigation.IdCategory==cat))
                                                )
                            .OrderBy(question => question.PublishDate)
                            .Skip(skip)
                            .Take(_takeAmount)
                            .ToListAsync();
                    case SortOptionEnum.Tags:
                        return await _context.Questions
                            .Include(question => question.QuestionCategories)
                            .Where(
                                question => _context.QuestionCategories
                                    .Include(qc => qc.IdCategoryNavigation.IdCategory)
                                    .Where(qc => qc.IdQuestion == question.IdQuestion)
                                    .Any(qc => categories.Any(cat => qc.IdCategoryNavigation.IdCategory == cat))
                                                )
                            .Skip(skip)
                            .Take(_takeAmount)
                            .ToListAsync();
                    case SortOptionEnum.Views:
                        return await _context.Questions
                            .Include(question => question.QuestionCategories)
                            .Where(
                                question => _context.QuestionCategories
                                    .Include(qc => qc.IdCategoryNavigation.IdCategory)
                                    .Where(qc => qc.IdQuestion == question.IdQuestion)
                                    .Any(qc => categories.Any(cat => qc.IdCategoryNavigation.IdCategory == cat))
                            )
                            .OrderBy(question => question.Views)
                            .Skip(skip)
                            .Take(_takeAmount)
                            .ToListAsync();
                    case SortOptionEnum.Answers:
                        return await _context.Questions
                            .Include(question => question.QuestionCategories)
                            .Include(question => question.Answers)
                            .Where(
                                question => _context.QuestionCategories
                                    .Include(qc => qc.IdCategoryNavigation.IdCategory)
                                    .Where(qc => qc.IdQuestion == question.IdQuestion)
                                    .Any(qc => categories.Any(cat => qc.IdCategoryNavigation.IdCategory == cat))
                            )
                            .OrderBy(question => question.Answers.Count)
                            .Skip(skip)
                            .Take(_takeAmount)
                            .ToListAsync();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public async Task<Question> GetQuestion(int questionId)
        {
            var Question =  await _context.Questions.Where(question => question.IdQuestion == questionId).SingleAsync();
            if(Question != null)
                Question.Views++;
            await _context.SaveChangesAsync();
            return Question;
        }

        public async Task<QuestionStatusResult> AddQuestion(AddQuestionDTO question)
        {
            var Question = new Question()
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
            return await _context.SaveChangesAsync() == 1? QuestionStatusResult.QUESTION_ADDED : QuestionStatusResult.QUESTION_DATABASE_ERROR;
        }

        public async Task<QuestionStatusResult> UpdateQuestion(int questionId, UpdateQuestionDTO question)
        {
            var Question = await _context.Questions.Where(question => question.IdQuestion == questionId).SingleAsync();
            if (Question == null) return QuestionStatusResult.QUESTION_NOT_FOUND;
            Question.Description = question.Description;
            Question.IsModified = true;
            return await _context.SaveChangesAsync() == 1 ? QuestionStatusResult.QUESTION_UPDATED: QuestionStatusResult.QUESTION_DATABASE_ERROR;
        }

        public async Task<QuestionStatusResult> RemoveQuestion(int questionId)
        {
            var Question = await _context.Questions.Where(question => question.IdQuestion == questionId).SingleAsync();
            if(Question == null) return QuestionStatusResult.QUESTION_NOT_FOUND;
            _context.Remove(Question);
            
            return await _context.SaveChangesAsync() ==1 ? QuestionStatusResult.QUESTION_DELETED: QuestionStatusResult.QUESTION_DATABASE_ERROR;
        }

        public async Task<QuestionStatusResult> SetToCheck(int questionId, bool value)
        {
            var Question = await _context.Questions.Where(question => question.IdQuestion == questionId).SingleAsync();
            if (Question == null) return QuestionStatusResult.QUESTION_NOT_FOUND;
            Question.ToCheck = value;
            ;
            return await _context.SaveChangesAsync() ==1 ? QuestionStatusResult.QUESTION_CHANGED_TOCHECK_STATUS : QuestionStatusResult.QUESTION_DATABASE_ERROR;

        }

        public async Task<QuestionStatusResult> SetFinished(int questionId)
        {
            var Question = await _context.Questions.Where(question => question.IdQuestion == questionId).SingleAsync();
            if (Question == null) return QuestionStatusResult.QUESTION_NOT_FOUND;
            Question.IsFinished = true;
            return await _context.SaveChangesAsync() == 1 ? QuestionStatusResult.QUESTION_SET_TO_FINISHED : QuestionStatusResult.QUESTION_DATABASE_ERROR;
        }
    }
}
