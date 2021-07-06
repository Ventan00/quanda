using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quanda.Client.Helpers;
using Quanda.Client.Repositories.Interfaces;
using Quanda.Shared.DTOs.Responses;
using Quanda.Shared.Models;

namespace Quanda.Client.Repositories.Implementations
{
    public class CategoriesRepository : ICategoriesReposiotry
    {
        private const string ApiUrl = "/api/categories";
        private readonly IHttpService _httpService;

        public CategoriesRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }
        public async Task<List<CategoriesResponseDTO>> GetCategories()
        {
            var response = await _httpService.Get<List<CategoriesResponseDTO>>(ApiUrl);
            
            Console.WriteLine("response:"+response);
            return response.Response;
        }
    }
}
