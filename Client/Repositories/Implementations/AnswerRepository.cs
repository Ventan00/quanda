using Microsoft.AspNetCore.WebUtilities;
using Quanda.Client.Helpers;
using Quanda.Client.Repositories.Interfaces;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quanda.Client.Repositories.Implementations
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly IHttpService _httpService;
        private readonly string url = "api/answers";

        public AnswerRepository(IHttpService httpService)
        {
            this._httpService = httpService;
        }

        public async Task<List<AnswerResponseDTO>> GetAnswersAsync(int idQuestion, AnswersPageDTO productParams)
        {
            var queryStringParam = new Dictionary<string, string>
            {
                ["pageSize"] = productParams.PageSize.ToString(),
                ["startIndex"] = productParams.StartIndex.ToString()
            };
            var response = await _httpService.Get<List<AnswerResponseDTO>>(QueryHelpers.AddQueryString($"api/answers/{idQuestion}", queryStringParam));
            return response.Response;
        }

        public async Task<AnswerResponseDTO> GetAnswerAsync(int idAnswer)
        {
            var response = await _httpService.Get<AnswerResponseDTO>($"{url}/{idAnswer}/details");
            return response.Response;
        }

        public async Task<Tuple<bool, string>> UpdateRatingAnswerAsync(int idAnswer, int rating)
        {
            UpdateRatingAnswerDTO updateRatingDto = new()
            {
                Rating = rating
            };
            var response = await _httpService.Post<UpdateRatingAnswerDTO>($"{url}/{idAnswer}/rating", updateRatingDto);
            if (!response.Success)
            {
                return new(false, await response.GetBody());
            }

            return new(true, null);
        }

        public async Task<Tuple<bool, string>> DeleteAnswer(int idAnswer)
        {
            var response = await _httpService.Delete($"{url}/{idAnswer}");
            if (!response.Success)
                return new(false, await response.GetBody());
            return new(true, null);
        }

        public async Task<Tuple<bool, string>> UpdateAnswer(int idAnswer, String text)
        {
            UpdateAnswerDTO updateAnswerDTO = new()
            {
                Text = text
            };

            var response = await _httpService.Put<UpdateAnswerDTO>($"{url}/{idAnswer}", updateAnswerDTO);
            if (!response.Success)
                return new(false, await response.GetBody());
            return new(true, null);
        }

        public async Task<Tuple<bool, string>> AddAnswer(string text, int idQuestion, int idRootAnswer)
        {
            AddAnswerDTO addAnswerDTO = new()
            {
                Text = text,
                IdQuestion = idQuestion,
                IdRootAnswer = idRootAnswer == 0 ? null : idRootAnswer
            };
            var response = await _httpService.Post<AddAnswerDTO>($"{url}", addAnswerDTO);
            if (!response.Success)
                return new(false, await response.GetBody());
            return new(true, null);
        }

    }
}
