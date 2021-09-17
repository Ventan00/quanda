using Quanda.Client.Helpers;
using Quanda.Shared.DTOs.Responses;
using Quanda.Shared.Enums;
using System.Threading.Tasks;

namespace Quanda.Client.Repositories
{
    /// <summary>
    ///     Repozytorium odpowiedzialne za tagi.
    /// </summary>
    public interface ITagsRepository
    {
        /// <summary>
        ///     Zwraca tagi z danej strony.
        /// </summary>
        /// <param name="page">Aktualnie przeglądana strona tagów.</param>
        /// <param name="sortOption">Rodzaj sortowania tagów.</param>
        /// <returns>TagsPageResponseDTO</returns>
        public Task<TagsPageResponseDTO> GetTagsAsync(int page, SortTagsEnum sortOption);

        /// <summary>
        ///     Zwraca subtagi z danej strony.
        /// </summary>
        /// <param name="idMainTag">Id tagu nadrzędnego.</param>
        /// <param name="page">Aktualnie przeglądana strona subtagów.</param>
        /// <param name="sortOption">Rodzaj sortowania subtagów.</param>
        /// <returns>SubTagsPageResponseDTO</returns>
        public Task<SubTagsPageResponseDTO> GetSubTagsAsync(int idMainTag, int page, SortTagsEnum sortOption);

    }
    public class TagsRepository : ITagsRepository
    {
        /// <summary>
        ///     Ścieżka do api kontrolera
        /// </summary>
        private const string ApiUrl = "/api/tags";

        private readonly IHttpService _httpService;

        public TagsRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<TagsPageResponseDTO> GetTagsAsync(int page, SortTagsEnum sortOption)
        {
            var response = await _httpService.Get<TagsPageResponseDTO>($"{ApiUrl}?page={page}&sortOption={(int)sortOption}");

            return response.Response;
        }

        public async Task<SubTagsPageResponseDTO> GetSubTagsAsync(int idMainTag, int page, SortTagsEnum sortOption)
        {
            var response = await _httpService.Get<SubTagsPageResponseDTO>($"{ApiUrl}/{idMainTag}?page={page}&sortOption={(int)sortOption}");

            return response.Response;
        }
    }
}