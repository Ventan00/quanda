using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.Models;

namespace Quanda.Server.Repositories.Interfaces
{
    public interface IQuestionRepository
    {
        Task<List<Question>> GetQuestions(GetQuestionsDTO getQuestionsDto);
        Task<Question> GetQuestion(int questionId);
        Task<int> AddQuestion(AddQuestionDTO question);
        Task<int> UpdateQuestion(int questionId, UpdateQuestionDTO question);
        Task<int> RemoveQuestion(int questionId);
        Task<int> SetToCheck(int questionId,bool value);
        Task<int> SetFinished(int questionId);
    }
}
