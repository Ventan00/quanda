﻿using Quanda.Client.Helpers;
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
        private readonly IHttpService httpService;
        private readonly string url = "api/answers";

        public AnswerRepository(IHttpService httpService)
        {
            this.httpService = httpService;
        }

        public async Task<List<AnswerBoxResponseDto>> GetAnswersAsync(int idQuestion)
        {
            return (await httpService.Get<List<AnswerBoxResponseDto>>($"{url}/{idQuestion}")).Response;
        }

        public async Task<Tuple<bool, string>> UpdateRatingAnswerAsync(int idAnswer, int rating)
        {
            UpdateRatingAnswerDTO updateRatingDto = new ()
            {
                Rating = rating
            };
            var response = await httpService.Post<UpdateRatingAnswerDTO>($"{url}/{idAnswer}/rating", updateRatingDto);
            if (!response.Success)
            {
                return new(false, await response.GetBody());
            }

            return new(true,null);
        }
    }
}
