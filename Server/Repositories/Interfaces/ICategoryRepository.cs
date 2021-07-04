using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quanda.Shared.Models;

namespace Quanda.Server.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategoriesAsync();
        Task<List<Category>> GetCategoriesOfQuestinAsync(int idQuestion);
    }
}
