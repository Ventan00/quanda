using System.Collections.Generic;
using System.Threading.Tasks;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.DTOs.Responses;
using Quanda.Shared.Enums;

namespace Quanda.Server.Repositories.Interfaces
{
    /// <summary>
    ///     Repozytorium odpowiedzialne za kategorie
    /// </summary>
    public interface ICategoryRepository
    {
        /// <summary>
        ///     Zwraca listę wszystkich kategorii
        /// </summary>
        /// <returns>List(CategoriesResponseDTO)</returns>
        Task<List<CategoriesResponseDTO>> GetCategoriesAsync();

        /// <summary>
        ///     Zwraca listę kategorii danego pytania
        /// </summary>
        /// <param name="idQuestion">ID pytania w BD</param>
        /// <returns>List(CategoriesResponseDTO)</returns>
        Task<List<CategoriesResponseDTO>> GetCategoriesOfQuestionAsync(int idQuestion);

        /// <summary>
        ///     Uaktualnia informacje o kategorii
        /// </summary>
        /// <param name="category">Obiekt DTO opisujący kategorię</param>
        /// <param name="idCategory">Id kategorii w BD</param>
        /// <returns>CategoryResultEnum</returns>
        Task<CategoryResultEnum> UpdateCategoryAsync(UpdateCategoryDTO category, int idCategory);

        /// <summary>
        ///     Dodaję kategorię do BD
        /// </summary>
        /// <param name="category">Obiekt DTO opisujący kategorię</param>
        /// <returns>CategoryResultEnum</returns>
        Task<CategoryResultEnum> AddCategoryAsync(AddCategoryDTO category);

        /// <summary>
        ///     Usuwa kategorię z BD
        /// </summary>
        /// <param name="idCategory">Id kategorii w BD</param>
        /// <returns>CategoryResultEnum</returns>
        Task<CategoryResultEnum> DeleteCategoryAsync(int idCategory);
    }
}