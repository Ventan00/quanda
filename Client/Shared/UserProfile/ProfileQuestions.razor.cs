using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Quanda.Client.Repositories.Interfaces;
using Quanda.Shared;
using Quanda.Shared.DTOs.Responses;

namespace Quanda.Client.Shared.UserProfile
{
    public partial class ProfileQuestions
    {
        private int _actualPage;

        private int _amountOfAllQuestions;
        private IEnumerable<QuestionInProfileResponseDto> _questions;

        [Parameter] public int IdUser { get; set; }

        [Inject] public IQuestionsReposiotry QuestionsReposiotry { get; set; }

        [Inject] public IJSRuntime JsRuntime { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await FetchMoreData();
        }

        private async Task FetchMoreData(int skip = 0)
        {
            _questions = await QuestionsReposiotry.GetUserQuestionsAsync(IdUser, skip);
            _amountOfAllQuestions = await QuestionsReposiotry.GetAmountOfUserQuestionsAsync(IdUser);
        }

        private async Task OnPageChange(int pageNumber)
        {
            _actualPage = pageNumber;
            _questions = null;
            await JsRuntime.InvokeVoidAsync("window.scrollTo", new
            {
                top = 0,
                behavior = "smooth"
            });
            await FetchMoreData(pageNumber * Config.ProfileQuestionsPageSize);
        }
    }
}