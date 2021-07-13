using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quanda.Server.Data;
using Quanda.Server.Repositories.Interfaces;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.DTOs.Responses;
using Quanda.Shared.Enums;
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
        public async Task<List<CategoriesResponseDTO>> GetCategoriesAsync()
        {
            return await _context.Categories
                .Select(category => new CategoriesResponseDTO()
                {
                    IdCategory = category.IdCategory,
                    IdMainCategory = category.IdMainCategory,
                    Name = category.Name
                })
                .ToListAsync();
        }

        public async Task<List<CategoriesResponseDTO>> GetCategoriesOfQuestionAsync(int idQuestion)
        {
            return await _context.QuestionCategories
                .Include(qc => qc.IdCategoryNavigation)
                .Where(qc => qc.IdQuestion == idQuestion)
                .Select(qc => qc.IdCategoryNavigation)
                .Select(category => new CategoriesResponseDTO()
                {
                    IdCategory = category.IdCategory,
                    IdMainCategory = category.IdMainCategory,
                    Name = category.Name
                })
                .ToListAsync();
        }

        public async Task<CategoryResultEnum> UpdateCategoryAsync(UpdateCategoryDTO category, int idCategory)
        {
            var Cat = await _context.Categories.Where(cat => cat.IdCategory == idCategory).SingleAsync();
            if (Cat == null)
                return CategoryResultEnum.CATEGORY_NOT_FOUND;
            Cat.IdMainCategory = category.IdMainCategory;
            Cat.Name = category.Name;
            return await _context.SaveChangesAsync() == 1 ? CategoryResultEnum.CATEGORY_UPDATED : CategoryResultEnum.CATEGORY_DATABASE_ERROR;
        }

        public async Task<CategoryResultEnum> AddCategoryAsync(AddCategoryDTO category)
        {
            Category cat = new Category
            {
                IdMainCategory = category.IdMainCategory,
                Name = category.Name
            };
            await _context.AddAsync(cat);
            return await _context.SaveChangesAsync() == 1
                ? CategoryResultEnum.CATEGORY_CREATED
                : CategoryResultEnum.CATEGORY_DATABASE_ERROR;
        }

        public async Task<CategoryResultEnum> DeleteCategoryAsync(int idCategory)
        {
            var Cat = await _context.Categories.Where(cat => cat.IdCategory == idCategory).SingleAsync();
            if (Cat == null)
                return CategoryResultEnum.CATEGORY_NOT_FOUND;
            _context.Remove(Cat);
            return await _context.SaveChangesAsync() == 1
                ? CategoryResultEnum.CATEGORY_DELETED
                : CategoryResultEnum.CATEGORY_DATABASE_ERROR;
        }
    }
}
