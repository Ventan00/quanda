using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.DTOs.Responses;
using Quanda.Shared.Enums;
using Quanda.Shared.Models;

namespace Quanda.Server.Repositories.Interfaces
{
    public interface ICategoryRepository
    {
        Task<List<CategoriesResponseDTO>> GetCategoriesAsync();
        Task<List<CategoriesResponseDTO>> GetCategoriesOfQuestionAsync(int idQuestion);
        Task<CategoryResultEnum> UpdateCategoryAsync(UpdateCategoryDTO category, int idCategory);
        Task<CategoryResultEnum> AddCategoryAsync(AddCategoryDTO category);
        Task<CategoryResultEnum> DeleteCategoryAsync(int idCategory);
    }
}
