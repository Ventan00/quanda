using System.Collections.Generic;
using System.Threading.Tasks;
using Quanda.Shared.DTOs.Responses;

namespace Quanda.Client.Repositories.Interfaces
{
    /// <summary>
    ///     Repozytorium odpowiedzialne za kategorie
    /// </summary>
    public interface ICategoriesReposiotry
    {
        /// <summary>
        ///     Zwraca listę wszystkich kategorii w BD
        /// </summary>
        /// <returns>List(CategoriesResponseDTO)</returns>
        public Task<List<CategoriesResponseDTO>> GetCategories();
    }
}