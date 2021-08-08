using Quanda.Shared.DTOs.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quanda.Client.Repositories.Interfaces
{
    /// <summary>
    ///     Repozytorium odpowiedzialne za tagi
    /// </summary>
    public interface ITagsReposiotry
    {
        /// <summary>
        ///     Zwraca listę wszystkich tagów w BD
        /// </summary>
        /// <returns>List(TagResponseDTO)</returns>
        public Task<List<TagResponseDTO>> GetTags();
    }
}