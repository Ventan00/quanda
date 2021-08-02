using Microsoft.EntityFrameworkCore;
using Quanda.Server.Data;
using Quanda.Server.Repositories.Interfaces;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.DTOs.Responses;
using Quanda.Shared.Enums;
using Quanda.Shared.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            return await _context.Tags
                .Select(category => new CategoriesResponseDTO
                {
                    IdCategory = category.IdTag,
                    IdMainCategory = category.IdMainTag,
                    Name = category.Name
                })
                .ToListAsync();
        }

        public async Task<List<CategoriesResponseDTO>> GetCategoriesOfQuestionAsync(int idQuestion)
        {
            return await _context.QuestionTags
                .Include(qc => qc.IdTagNavigation)
                .Where(qc => qc.IdQuestion == idQuestion)
                .Select(qc => qc.IdTagNavigation)
                .Select(category => new CategoriesResponseDTO
                {
                    IdCategory = category.IdTag,
                    IdMainCategory = category.IdMainTag,
                    Name = category.Name
                })
                .ToListAsync();
        }

        public async Task<CategoryResultEnum> UpdateCategoryAsync(UpdateCategoryDTO category, int idCategory)
        {
            var Cat = await _context.Tags.Where(cat => cat.IdTag == idCategory).SingleAsync();
            if (Cat == null)
                return CategoryResultEnum.CATEGORY_NOT_FOUND;
            Cat.IdMainTag = category.IdMainCategory;
            Cat.Name = category.Name;
            return await _context.SaveChangesAsync() == 1
                ? CategoryResultEnum.CATEGORY_UPDATED
                : CategoryResultEnum.CATEGORY_DATABASE_ERROR;
        }

        public async Task<CategoryResultEnum> AddCategoryAsync(AddCategoryDTO category)
        {
            var cat = new Tag
            {
                IdMainTag = category.IdMainCategory,
                Name = category.Name
            };
            await _context.AddAsync(cat);
            return await _context.SaveChangesAsync() == 1
                ? CategoryResultEnum.CATEGORY_CREATED
                : CategoryResultEnum.CATEGORY_DATABASE_ERROR;
        }

        public async Task<CategoryResultEnum> DeleteCategoryAsync(int idCategory)
        {
            var Cat = await _context.Tags.Where(cat => cat.IdTag == idCategory).SingleAsync();
            if (Cat == null)
                return CategoryResultEnum.CATEGORY_NOT_FOUND;
            _context.Remove(Cat);
            return await _context.SaveChangesAsync() == 1
                ? CategoryResultEnum.CATEGORY_DELETED
                : CategoryResultEnum.CATEGORY_DATABASE_ERROR;
        }
    }
}