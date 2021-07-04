using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quanda.Server.Data;
using Quanda.Server.Repositories.Interfaces;
using Quanda.Shared.Models;

namespace Quanda.Server.Repositories.Implementations
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<List<Category>> GetCategoriesOfQuestinAsync(int idQuestion)
        {
            return await _context.QuestionCategories
                .Include(qc => qc.IdCategoryNavigation)
                .Where(qc => qc.IdQuestion == idQuestion)
                .Select(qc => qc.IdCategoryNavigation)
                .ToListAsync();
        }
    }
}
