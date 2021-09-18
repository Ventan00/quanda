using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quanda.Shared.DTOs.Responses;

namespace Quanda.Client.Shared.RightMenu
{
    public partial class RightStandard
    {
        /// <summary>
        ///     Lista przechowująca top 3 użytkowników pod kontem punktów
        /// </summary>
        public IEnumerable<Top3UserResponseDTO> Top3Users;
        protected async override Task OnInitializedAsync()
        {
            Top3Users = await _usersRepository.GetTop3UsersAsync();
        }
    }
}
