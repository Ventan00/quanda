using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.DTOs.Responses;
using Quanda.Shared.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quanda.Server.Repositories.Interfaces
{
    /// <summary>
    ///     Repozytorium odpowiedzialne za tagi
    /// </summary>
    public interface ITagRepository
    {
        /// <summary>
        ///     Zwraca tagi z podanej strony.
        /// </summary>
        /// <param name="page">Aktualna strona przeglądanych tagów.</param>
        /// <returns>TagsPageResponseDTO</returns>
        Task<TagsPageResponseDTO> GetTagsAsync(int page);

        /// <summary>
        ///     Zwraca subtagi z podanej strony.
        /// </summary>
        /// <param name="idMainTag">Id nadrzędnego tagu.</param>
        /// <param name="page">Aktualna strona przeglądanych subtagów.</param>
        /// <returns>SubTagsPageResponseDTO</returns>
        Task<SubTagsPageResponseDTO> GetSubTagsAsync(int idMainTag, int page);

        /// <summary>
        ///     Zwraca listę tagów danego pytania
        /// </summary>
        /// <param name="idQuestion">ID pytania w BD</param>
        /// <returns>List(TagResponseDTO)</returns>
        Task<List<TagResponseDTO>> GetTagsOfQuestionAsync(int idQuestion);

        /// <summary>
        ///     Uaktualnia informacje o tagu
        /// </summary>
        /// <param name="tag">Obiekt DTO opisujący tag</param>
        /// <param name="idTag">Id tagu w BD</param>
        /// <returns>TagResultEnum</returns>
        Task<TagResultEnum> UpdateTagAsync(UpdateTagDTO tag, int idTag);

        /// <summary>
        ///     Dodaję tag do BD
        /// </summary>
        /// <param name="category">Obiekt DTO opisujący tag</param>
        /// <returns>TagResultEnum</returns>
        Task<TagResultEnum> AddTagAsync(AddTagDTO category);

        /// <summary>
        ///     Usuwa tag z BD
        /// </summary>
        /// <param name="idTag">Id tagu w BD</param>
        /// <returns>TagResultEnum</returns>
        Task<TagResultEnum> DeleteTagAsync(int idTag);
    }
}