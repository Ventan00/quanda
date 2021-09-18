using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace Quanda.Client.Pages.Users
{
    public partial class UserProfile
    {
        private bool _ownProfile;

        private bool _showQuestions = true;

        [Parameter] public int IdUser { get; set; }

        [CascadingParameter] public Task<AuthenticationState> AuthenticationStateTask { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var authState = await AuthenticationStateTask;
            _ownProfile = int.Parse(authState.User.FindFirst(ClaimTypes.NameIdentifier).Value) == IdUser;
        }

        protected override void OnAfterRender(bool firstRender)
        {
            Console.WriteLine("UserProfile page - render");
        }

        private void ShowQuestions()
        {
            _showQuestions = true;
        }

        private void ShowAnswers()
        {
            _showQuestions = false;
        }
    }
}