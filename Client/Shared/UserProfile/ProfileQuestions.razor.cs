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

        private GetProfileQuestionsResponseDto _profileQuestions;

        [Parameter] public int IdUser { get; set; }

        [Inject] public IQuestionsReposiotry QuestionsReposiotry { get; set; }

        [Inject] public IJSRuntime JsRuntime { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await FetchMoreData();
        }

        private async Task FetchMoreData(int skip = 0)
        {
            _profileQuestions = new GetProfileQuestionsResponseDto();
            _profileQuestions = await QuestionsReposiotry.GetUserQuestionsAsync(IdUser, skip);
        }

        private async Task OnPageChange(int pageNumber)
        {
            _actualPage = pageNumber;
            await JsRuntime.InvokeVoidAsync("window.scrollTo", new
            {
                top = 0,
                behavior = "smooth"
            });
            await FetchMoreData(pageNumber * Config.ProfileQuestionsPageSize);
        }
    }
}