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
        public QuestionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Question>> GetQuestions(GetQuestionsDTO getQuestionsDto)
        {
            if (getQuestionsDto.Categories.Count() == 0)
            {
                switch (getQuestionsDto.SortingOption)
                {
                    case SORT_OPTION_ENUM.Date:
                        return await _context.Questions
                            .OrderBy(question => question.PublishDate)
                            .Skip(getQuestionsDto.Skip)
                            .Take(10)
                            .ToListAsync();
                    case SORT_OPTION_ENUM.Tags:
                        return null;
                    case SORT_OPTION_ENUM.Views:
                        return await _context.Questions
                            .OrderBy(question => question.Views)
                            .Skip(getQuestionsDto.Skip)
                            .Take(10)
                            .ToListAsync();
                    case SORT_OPTION_ENUM.Answers:
                        return await _context.Questions
                            .Include(question => question.Answers)
                            .OrderBy(question => question.Views)
                            .Skip(getQuestionsDto.Skip)
                            .Take(10)
                            .ToListAsync();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                switch (getQuestionsDto.SortingOption)
                {
                    case SORT_OPTION_ENUM.Date:
                        return await _context.Questions
                            .Include(question => question.QuestionCategories)
                            .Where(
                                question => _context.QuestionCategories
                                    .Include(qc => qc.IdCategoryNavigation)
                                    .Where(qc => qc.IdQuestion==question.IdQuestion)
                                    .Any(qc => getQuestionsDto.Categories.Any(cat => qc.IdCategoryNavigation==cat))
                                                )
                            .OrderBy(question => question.PublishDate)
                            .Skip(getQuestionsDto.Skip)
                            .Take(10)
                            .ToListAsync();
                    case SORT_OPTION_ENUM.Tags:
                        return await _context.Questions
                            .Include(question => question.QuestionCategories)
                            .Where(
                                question => _context.QuestionCategories
                                    .Include(qc => qc.IdCategoryNavigation)
                                    .Where(qc => qc.IdQuestion == question.IdQuestion)
                                    .Any(qc => getQuestionsDto.Categories.Any(cat => qc.IdCategoryNavigation == cat))
                                                )
                            .Skip(getQuestionsDto.Skip)
                            .Take(10)
                            .ToListAsync();
                    case SORT_OPTION_ENUM.Views:
                        return await _context.Questions
                            .Include(question => question.QuestionCategories)
                            .Where(
                                question => _context.QuestionCategories
                                    .Include(qc => qc.IdCategoryNavigation)
                                    .Where(qc => qc.IdQuestion == question.IdQuestion)
                                    .Any(qc => getQuestionsDto.Categories.Any(cat => qc.IdCategoryNavigation == cat))
                            )
                            .OrderBy(question => question.Views)
                            .Skip(getQuestionsDto.Skip)
                            .Take(10)
                            .ToListAsync();
                    case SORT_OPTION_ENUM.Answers:
                        return await _context.Questions
                            .Include(question => question.QuestionCategories)
                            .Include(question => question.Answers)
                            .Where(
                                question => _context.QuestionCategories
                                    .Include(qc => qc.IdCategoryNavigation)
                                    .Where(qc => qc.IdQuestion == question.IdQuestion)
                                    .Any(qc => getQuestionsDto.Categories.Any(cat => qc.IdCategoryNavigation == cat))
                            )
                            .OrderBy(question => question.Answers.Count)
                            .Skip(getQuestionsDto.Skip)
                            .Take(10)
                            .ToListAsync();
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public async Task<Question> GetQuestion(int questionId)
        {
            return await _context.Questions.Where(question => question.IdQuestion == questionId).SingleAsync();
        }

        public async Task<QuestionResult> AddQuestion(AddQuestionDTO question)
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
            return await _context.SaveChangesAsync() == 1? QuestionResult.QUESTION_ADDED : QuestionResult.QUESTION_DATABASE_ERROR;
        }

        public async Task<QuestionResult> UpdateQuestion(int questionId, UpdateQuestionDTO question)
        {
            var Question = await _context.Questions.Where(question => question.IdQuestion == questionId).SingleAsync();
            if (Question == null) return QuestionResult.QUESTION_NOT_FOUND;
            Question.Description = question.Description;
            Question.IsModified = true;
            return await _context.SaveChangesAsync() == 1 ? QuestionResult.QUESTION_UPDATED: QuestionResult.QUESTION_DATABASE_ERROR;
        }

        public async Task<QuestionResult> RemoveQuestion(int questionId)
        {
            var Question = await _context.Questions.Where(question => question.IdQuestion == questionId).SingleAsync();
            if(Question == null) return QuestionResult.QUESTION_NOT_FOUND;
            _context.Remove(Question);
            
            return await _context.SaveChangesAsync() ==1 ? QuestionResult.QUESTION_DELETED: QuestionResult.QUESTION_DATABASE_ERROR;
        }

        public async Task<QuestionResult> SetToCheck(int questionId, bool value)
        {
            var Question = await _context.Questions.Where(question => question.IdQuestion == questionId).SingleAsync();
            if (Question == null) return QuestionResult.QUESTION_NOT_FOUND;
            Question.ToCheck = value;
            ;
            return await _context.SaveChangesAsync() ==1 ? QuestionResult.QUESTION_CHANGED_TOCHECK_STATUS : QuestionResult.QUESTION_DATABASE_ERROR;

        }

        public async Task<QuestionResult> SetFinished(int questionId)
        {
            var Question = await _context.Questions.Where(question => question.IdQuestion == questionId).SingleAsync();
            if (Question == null) return QuestionResult.QUESTION_NOT_FOUND;
            Question.IsFinished = true;
            return await _context.SaveChangesAsync() == 1 ? QuestionResult.QUESTION_SET_TO_FINISHED : QuestionResult.QUESTION_DATABASE_ERROR;
        }
    }
}
