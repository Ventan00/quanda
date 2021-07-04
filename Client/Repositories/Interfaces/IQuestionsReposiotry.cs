using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quanda.Shared.Enums;
using Quanda.Shared.Models;

namespace Quanda.Client.Repositories.Interfaces
{
    public interface IQuestionsReposiotry
    {
        public Task<List<Question>> GetQuestions(int page, SORT_OPTION_ENUM sortingBy, List<Category> categories);
    }
}
