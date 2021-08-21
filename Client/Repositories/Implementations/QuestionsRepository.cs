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

        public async Task<QuestionResponseDTO> GetQuestion(int idQuestion)
        {
            var response = await _httpService.Get<QuestionResponseDTO>($"{ApiUrl}/{idQuestion}");
            if (!response.Success)
                return null;
            return response.Response;
        }

        public async Task<GetQuestionsDTO> GetQuestions(int page, SortOptionEnum sortingBy, List<int> tags)
        {
            var url = $"{ApiUrl}?skip={page * _skipAmount}&sortOption={Enum.GetName(sortingBy)}";
            if (tags.Count != 0) url += $"&tag={string.Join("&tag=", tags)}";
            var response = await _httpService.Get<GetQuestionsDTO>(url);

            return response.Response;
        }

        public async Task<int> GetQuestionsAmount(List<int> tags)
        {
            var url = $"{ApiUrl}/count?";
            if (tags.Count != 0) url += $"tag={string.Join("&tag=", tags)}";
            var response = await _httpService.Get<int>(url);
            return response.Response;
        }
    }
}