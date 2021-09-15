using Quanda.Shared.DTOs.Responses;
using System.Threading.Tasks;

namespace Quanda.Client.Repositories.Interfaces
{
    /// <summary>
    ///     Repozytorium odpowiedzialne za tagi.
    /// </summary>
    public interface ITagsReposiotry
    {
        /// <summary>
        ///     Zwraca tagi z danej strony.
        /// </summary>
        /// <param name="page">Aktualnie przeglądana strona tagów.</param>
        /// <returns>TagsPageResponseDTO</returns>
        public Task<TagsPageResponseDTO> GetTagsAsync(int page);

        /// <summary>
        ///     Zwraca subtagi z danej strony.
        /// </summary>
        /// <param name="idMainTag">Id tagu nadrzędnego.</param>
        /// <param name="page">Aktualnie przeglądana strona subtagów.</param>
        /// <returns>SubTagsPageResponseDTO</returns>
        public Task<SubTagsPageResponseDTO> GetSubTagsAsync(int idMainTag, int page);

    }
}