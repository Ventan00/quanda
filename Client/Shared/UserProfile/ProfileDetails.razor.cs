using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Quanda.Client.Repositories.Interfaces;
using Quanda.Shared.DTOs.Responses;

namespace Quanda.Client.Shared.UserProfile
{
    public partial class ProfileDetails
    {
        private UserProfileDetailsResponseDto _userProfileDetails;

        [Parameter] public bool OwnProfile { get; set; }

        [Parameter] public int IdUser { get; set; }

        [Inject] public IUsersRepository UsersRepository { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _userProfileDetails = await UsersRepository.GetUserProfileDetailsAsync(IdUser);
        }

        private void ViewAllPoints()
        {
            throw new NotImplementedException("Waiting for UI template");
        }

        private void EditProfile()
        {
            throw new NotImplementedException();
        }
    }
}