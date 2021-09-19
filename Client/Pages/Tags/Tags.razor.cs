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
    public partial class Tags : IAsyncDisposable
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
        public SortTagsEnum ActiveSorting { get; set; }

        /// <summary>
        ///     Metoda odczytuje z URI parametry oraz pobiera tagi.
        /// </summary>
        protected async override Task OnInitializedAsync()
        {
            await GetQueryValues();
            NavManager.LocationChanged += HandleLocationChanged;

        }
        /// <summary>
        ///     Metoda pobiera parametry z URI oraz aktualizuje zbiór tagów.
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

            await GetTags();
        }

        /// <summary>
        ///     Metoda odpowiedzialna za pobranie tagów z danej strony.
        /// </summary>
        private async Task GetTags()
        {
            TagPage = await TagRepository.GetTagsAsync(activePage, ActiveSorting);
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
        ///     Metoda odpowiedzialna za zmianę trybu sortowania tagów.
        /// </summary>
        private async Task ChangeSorting()
        {
            ActiveSorting = ActiveSorting == SortTagsEnum.Popular ? SortTagsEnum.Name : SortTagsEnum.Popular;

            var newUri = new Uri(NavManager.Uri).GetLeftPart(UriPartial.Path);
            var uri = NavManager.ToAbsoluteUri(NavManager.Uri);
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("page", out _))
                newUri = QueryHelpers.AddQueryString(newUri, "page", (activePage + 1).ToString());

            newUri = QueryHelpers.AddQueryString(newUri, "sortOption", ActiveSorting.ToString());
            NavManager.NavigateTo(newUri);
        }

        /// <summary>
        ///     Metoda aktualizująca aktualną stronę przeglądanych tagów.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private async Task SetPage(int page)
        {
            activePage = page;

            var newUri = new Uri(NavManager.Uri).GetLeftPart(UriPartial.Path);
            var uri = NavManager.ToAbsoluteUri(NavManager.Uri);
            newUri = QueryHelpers.AddQueryString(newUri, "page", (activePage + 1).ToString());
            if (QueryHelpers.ParseQuery(uri.Query).TryGetValue("sortOption", out _))
                newUri = QueryHelpers.AddQueryString(newUri, "sortOption", ActiveSorting.ToString());
            NavManager.NavigateTo(newUri);
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
