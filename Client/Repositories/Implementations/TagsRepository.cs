using Quanda.Client.Helpers;
using Quanda.Client.Repositories.Interfaces;
using Quanda.Shared.DTOs.Responses;
using System.Collections.Generic;
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

        public async Task<List<TagResponseDTO>> GetTags()
        {
            var response = await _httpService.Get<List<TagResponseDTO>>(ApiUrl);

            return response.Response;
        }
    }
}