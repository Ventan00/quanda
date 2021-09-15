using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Quanda.Client.Repositories.Interfaces;
using Quanda.Shared.DTOs.Responses;
using System;
using System.Threading.Tasks;

namespace Quanda.Client.Pages
{
    public partial class SubTags
    {
        [Inject] ITagsRepository TagRepository { get; set; }
        [Inject] NavigationManager NavManager { get; set; }

        /// <summary>
        ///     Id nadrzędnego tagu.
        /// </summary>
        [Parameter]
        public int IdMainTag { get; set; }
        /// <summary>
        ///     Aktualnie przeglądana strona
        /// </summary>
        private int activePage = 0;
        /// <summary>
        ///     SubTagi.
        /// </summary>
        public SubTagsPageResponseDTO SubTagPage { get; set; }
        /// <summary>
        ///     Aktywna opcja sortowania subtagów.
        /// </summary>
        public int ActiveSorting { get; set; }

        /// <summary>
        ///     Metoda odpowiedzialna za odczytanie strony z URI oraz pobranie subtagów.
        /// </summary>
        protected async override Task OnInitializedAsync()
        {
            var uri = NavManager.ToAbsoluteUri(NavManager.Uri);
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("page", out var initPage))
            {
                activePage = Convert.ToInt32(initPage);
            }
            await GetSubTags();
        }

        /// <summary>
        ///     Metoda odpowiedzialna za pobranie subtagów z danej strony.
        /// </summary>
        private async Task GetSubTags()
        {
            SubTagPage = await TagRepository.GetSubTagsAsync(IdMainTag, activePage);
        }

        /// <summary>
        ///     Metoda odpowiedzialna za zmianę trybu sortowania subtagów.
        /// </summary>
        private async Task ChangeSorting()
        {
            ActiveSorting = ActiveSorting == 0 ? 1 : 0;
        }

        /// <summary>
        ///     Metoda odpowiedzialna za aktualizację subtagów dla nowej strony.
        /// </summary>
        /// <param name="page">Nowa strona.</param>
        private async Task SetPage(int page)
        {
            activePage = page;
            StateHasChanged();
            await GetSubTags();
        }
    }
}