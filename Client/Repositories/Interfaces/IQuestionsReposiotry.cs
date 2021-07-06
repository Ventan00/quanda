using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quanda.Shared.DTOs.Responses;
using Quanda.Shared.Enums;
using Quanda.Shared.Models;

namespace Quanda.Client.Repositories.Interfaces
{
    public interface IQuestionsReposiotry
    {
        public Task<List<GetQuestionsDTO>> GetQuestions(int page, SortOptionEnum sortingBy, List<int> categories);
    }
}
