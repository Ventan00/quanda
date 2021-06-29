using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quanda.Server.Data;
using Quanda.Server.Repositories.Interfaces;
using Quanda.Shared.DTOs.Requests;
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
            throw new NotImplementedException();
        }

        public async Task<Question> GetQuestion(int questionId)
        {
            return await _context.Questions.Where(question => question.IdQuestion == questionId).SingleAsync();
        }

        public async Task<int> AddQuestion(AddQuestionDTO question)
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
            return await _context.SaveChangesAsync() == 1? 0 : 1;
        }

        public async Task<int> UpdateQuestion(int questionId, UpdateQuestionDTO question)
        {
            var Question = await _context.Questions.Where(question => question.IdQuestion == questionId).SingleAsync();
            if (Question == null) return 1;
            Question.Description = question.Description;
            Question.IsModified = true;
            return await _context.SaveChangesAsync() == 1 ? 0: 2;
        }

        public async Task<int> RemoveQuestion(int questionId)
        {
            var Question = await _context.Questions.Where(question => question.IdQuestion == questionId).SingleAsync();
            if(Question == null) return 1;
            _context.Remove(Question);
            await _context.SaveChangesAsync();
            return  0;
        }

        public async Task<int> SetToCheck(int questionId, bool value)
        {
            var Question = await _context.Questions.Where(question => question.IdQuestion == questionId).SingleAsync();
            if (Question == null) return 1;
            Question.ToCheck = value;
            await _context.SaveChangesAsync();
            return 0;

        }


        public async Task<int> SetFinished(int questionId)
        {
            throw new NotImplementedException();
        }
    }
}
