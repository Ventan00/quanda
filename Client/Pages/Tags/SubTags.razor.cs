using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Quanda.Client.Repositories;
using Quanda.Shared.DTOs.Responses;
using Quanda.Shared.Enums;
using System;
using System.Threading.Tasks;

namespace Quanda.Client.Pages.Tags
{
    public partial class SubTags : IAsyncDisposable
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
        public SortTagsEnum ActiveSorting { get; set; }

        /// <summary>
        ///     Metoda odczytuje z URI parametry oraz pobiera subtagi.
        /// </summary>
        protected async override Task OnInitializedAsync()
        {
            await GetQueryValues();
            NavManager.LocationChanged += HandleLocationChanged;
        }

        /// <summary>
        ///     Metoda pobiera parametry z URI oraz aktualizuje zbiór subtagów.
        /// </summary>
        private async Task GetQueryValues()
        {
            var uri = NavManager.ToAbsoluteUri(NavManager.Uri);
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("page", out var initPage))
            {
                var page = Convert.ToInt32(initPage);
                if (page < 1)
                    activePage = 0;
                else
                    activePage = page - 1;
            }
            else
                activePage = 0;

            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("sortOption", out var initSortOption))
            {
                if (Enum.TryParse(initSortOption, out SortTagsEnum activeSorting))
                    ActiveSorting = activeSorting;
                else
                    ActiveSorting = SortTagsEnum.Popular;
            }
            else
                ActiveSorting = SortTagsEnum.Popular;

            await GetSubTags();
        }

        /// <summary>
        ///     Metoda odpowiedzialna za pobranie subtagów z danej strony.
        /// </summary>
        private async Task GetSubTags()
        {
            SubTagPage = await TagRepository.GetSubTagsAsync(IdMainTag, activePage, ActiveSorting);
        }

        /// <summary>
        ///     Metoda wywołana w przypadku zmiany URI.
        /// </summary>
        private async void HandleLocationChanged(object sender, LocationChangedEventArgs e)
        {
            await GetQueryValues();
            StateHasChanged();
        }

        /// <summary>
        ///     Metoda odpowiedzialna za zmianę trybu sortowania subtagów.
        /// </summary>
        private async Task ChangeSorting()
        {
            ActiveSorting = ActiveSorting == SortTagsEnum.Popular ? SortTagsEnum.Name : SortTagsEnum.Popular;

            var newUri = new Uri(NavManager.Uri).GetLeftPart(UriPartial.Path);
            var uri = NavManager.ToAbsoluteUri(NavManager.Uri);
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("page", out _))
                newUri = QueryHelpers.AddQueryString(newUri, "page", (activePage + 1).ToString());

            newUri = QueryHelpers.AddQueryString(newUri, "sortOption", ActiveSorting.ToString());
            NavManager.NavigateTo(newUri, false);
        }

        /// <summary>
        ///     Metoda aktualizująca aktualną stronę przeglądanych subtagów..
        /// </summary>
        /// <param name="page">Nowa strona.</param>
        private async Task SetPage(int page)
        {
            activePage = page;

            var newUri = new Uri(NavManager.Uri).GetLeftPart(UriPartial.Path);
            var uri = NavManager.ToAbsoluteUri(NavManager.Uri);
            newUri = QueryHelpers.AddQueryString(newUri, "page", (activePage + 1).ToString());
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("sortOption", out _))
                newUri = QueryHelpers.AddQueryString(newUri, "sortOption", ActiveSorting.ToString());
            NavManager.NavigateTo(newUri, false);
        }

        /// <summary>
        ///     Metoda odpowiedziala za usunięcie składnika z interfejsu użytkownika.
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            NavManager.LocationChanged -= HandleLocationChanged;
        }
    }
}