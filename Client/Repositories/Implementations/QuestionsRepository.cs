using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Quanda.Client.Helpers;
using Quanda.Client.Repositories.Interfaces;
using Quanda.Shared;
using Quanda.Shared.DTOs.Responses;
using Quanda.Shared.Enums;

namespace Quanda.Client.Repositories.Implementations
{
    public class QuestionsRepository : IQuestionsReposiotry
    {
        /// <summary>
        ///     Ścieżka do kontrolera api
        /// </summary>
        private const string ApiUrl = "/api/questions";

        private readonly IHttpService _httpService;

        /// <summary>
        ///     Wartość pobrana z Configu opisująca ilość pytań na stronie
        /// </summary>
        private readonly int _skipAmount = Config.QUESTIONS_PAGINATION_TAKE_SKIP;

        public QuestionsRepository(IHttpService httpService)
        {
            _httpService = httpService;
        }

        public async Task<List<GetQuestionsDTO>> GetQuestions(int page, SortOptionEnum sortingBy, List<int> categories)
        {
            var url = $"{ApiUrl}?skip={page * _skipAmount}&sortOption={Enum.GetName(sortingBy)}";
            if (categories.Count != 0) url += $"&category={string.Join("&category=", categories)}";
            var response = await _httpService.Get<List<GetQuestionsDTO>>(url);

            return response.Response;
        }

        public async Task<int> GetQuestionsAmount(List<int> categories)
        {
            var url = $"{ApiUrl}/count?";
            if (categories.Count != 0) url += $"category={string.Join("&category=", categories)}";
            var response = await _httpService.Get<int>(url);
            return response.Response;
        }
    }
}