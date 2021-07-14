using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quanda.Client.Helpers;
using Quanda.Client.Repositories.Interfaces;
using Quanda.Shared.DTOs.Responses;

namespace Quanda.Client.Repositories.Implementations
{
    public class CategoriesRepository : ICategoriesReposiotry
    {
        /// <summary>
        ///     Ścieżka do api kontrolera
        /// </summary>
        private const string ApiUrl = "/api/categories";

        private readonly IHttpService _httpService;

        public CategoriesRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<List<CategoriesResponseDTO>> GetCategories()
        {
            var response = await _httpService.Get<List<CategoriesResponseDTO>>(ApiUrl);

            Console.WriteLine("response:" + response);
            return response.Response;
        }
    }
}