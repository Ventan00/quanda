﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quanda.Server.Utils;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.Models;

namespace Quanda.Server.Repositories.Interfaces
{
    public interface IQuestionRepository
    {
        Task<List<Question>> GetQuestions(GetQuestionsDTO getQuestionsDto);
        Task<Question> GetQuestion(int questionId);
        Task<QuestionResult> AddQuestion(AddQuestionDTO question);
        Task<QuestionResult> UpdateQuestion(int questionId, UpdateQuestionDTO question);
        Task<QuestionResult> RemoveQuestion(int questionId);
        Task<QuestionResult> SetToCheck(int questionId,bool value);
        Task<QuestionResult> SetFinished(int questionId);
    }
}
