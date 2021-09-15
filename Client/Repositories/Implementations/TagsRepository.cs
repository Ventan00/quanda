using Quanda.Client.Helpers;
using Quanda.Client.Repositories.Interfaces;
using Quanda.Shared.DTOs.Responses;
using System.Threading.Tasks;

namespace Quanda.Client.Repositories.Implementations
{
    public class TagsRepository : ITagsReposiotry
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

        public async Task<TagsPageResponseDTO> GetTagsAsync(int page)
        {
            var response = await _httpService.Get<TagsPageResponseDTO>($"{ApiUrl}?page={page}");

            return response.Response;
        }

        public async Task<SubTagsPageResponseDTO> GetSubTagsAsync(int idMainTag, int page)
        {
            var response = await _httpService.Get<SubTagsPageResponseDTO>($"{ApiUrl}/{idMainTag}?page={page}");

            return response.Response;
        }
    }
}