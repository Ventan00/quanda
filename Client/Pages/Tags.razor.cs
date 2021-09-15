using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Quanda.Client.Repositories.Interfaces;
using Quanda.Shared.DTOs.Responses;
using System;
using System.Threading.Tasks;

namespace Quanda.Client.Pages
{
    public partial class Tags
    {
        [Inject] ITagsRepository TagRepository { get; set; }
        [Inject] NavigationManager NavManager { get; set; }

        /// <summary>
        ///     Aktualnie przeglądana strona
        /// </summary>
        private int activePage = 0;
        /// <summary>
        ///     Tagi.
        /// </summary>
        /// <summary>
        public TagsPageResponseDTO TagPage { get; set; }
        ///     Aktywna opcja sortowania tagów.
        /// </summary>
        public int ActiveSorting { get; set; }

        /// <summary>
        ///     Metoda odpowiedzialna za odczytanie strony z URI oraz pobranie tagów.
        /// </summary>
        protected async override Task OnInitializedAsync()
        {

            var uri = NavManager.ToAbsoluteUri(NavManager.Uri);
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("page", out var initPage))
            {
                activePage = Convert.ToInt32(initPage);
            }
            await GetTags();
        }

        /// <summary>
        ///     Metoda odpowiedzialna za pobranie tagów z danej strony.
        /// </summary>
        private async Task GetTags()
        {
            TagPage = await TagRepository.GetTagsAsync(activePage);
        }

        /// <summary>
        ///     Metoda odpowiedzialna za zmianę trybu sortowania tagów.
        /// </summary>
        private async Task ChangeSorting()
        {
            ActiveSorting = ActiveSorting == 0 ? 1 : 0;
        }

        private async Task SetPage(int page)
        {
            activePage = page;
            StateHasChanged();
            await GetTags();
        }
    }
}