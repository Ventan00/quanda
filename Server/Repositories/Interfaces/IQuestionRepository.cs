using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quanda.Server.Utils;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.Enums;
using Quanda.Shared.Models;

namespace Quanda.Server.Repositories.Interfaces
{
    public interface IQuestionRepository
    {
        Task<List<Question>> GetQuestions(int skip, SortOptionEnum sortOption, List<int>? categories);
        Task<Question> GetQuestion(int questionId);
        Task<QuestionStatusResult> AddQuestion(AddQuestionDTO question);
        Task<QuestionStatusResult> UpdateQuestion(int questionId, UpdateQuestionDTO question);
        Task<QuestionStatusResult> RemoveQuestion(int questionId);
        Task<QuestionStatusResult> SetToCheck(int questionId,bool value);
        Task<QuestionStatusResult> SetFinished(int questionId);
    }
}
