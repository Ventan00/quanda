using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Quanda.Client.Helpers;
using Quanda.Client.Repositories.Interfaces;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.Enums;
using Quanda.Shared.Models;

namespace Quanda.Client.Repositories.Implementations
{
    public class QuestionsRepository : IQuestionsReposiotry
    {
        private const string ApiUrl = "/api/questions";
        private readonly IHttpService _httpService;
        private readonly int _skipAmount = 10;

        public QuestionsRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<List<Question>> GetQuestions(int page, SORT_OPTION_ENUM sortingBy, List<Category> categories)
        {
            var url = $"{ApiUrl}?skip={page*_skipAmount}&sortOption={Enum.GetName(sortingBy)}&category={string.Join("&category=",categories.Select(cat => cat.IdCategory).ToList())}";
            var response = await _httpService.Get<List<Question>>(ApiUrl);
            return response.Response;

        }

    }
}
