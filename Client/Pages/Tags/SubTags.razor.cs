using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Quanda.Client.Extensions;
using Quanda.Client.Repositories;
using Quanda.Shared.DTOs.Responses;
using Quanda.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quanda.Client.Pages.Tags
{
    public partial class SubTags : IAsyncDisposable
    {
        [Inject] private ITagsRepository TagRepository { get; set; }
        [Inject] private NavigationManager NavManager { get; set; }

        /// <summary>
        ///     Id nadrzędnego tagu.
        /// </summary>
        [Parameter]
        public int IdMainTag { get; set; }
        /// <summary>
        ///     Aktualnie przeglądana strona
        /// </summary>
        private int _activePage = 0;
        /// <summary>
        ///     SubTagi.
        /// </summary>
        private SubTagsPageResponseDTO _subTagPage;
        /// <summary>
        ///     Aktywna opcja sortowania subtagów.
        /// </summary>
        private SortTagsEnum _activeSorting;

        /// <summary>
        ///     Metoda odczytuje z URI parametry oraz pobiera subtagi.
        /// </summary>
        protected async override Task OnInitializedAsync()
        {
            _subTagPage = null;
            await GetQueryValues();
            NavManager.LocationChanged += HandleLocationChanged;
        }

        /// <summary>
        ///     Metoda pobiera parametry z URI oraz aktualizuje zbiór subtagów.
        /// </summary>
        private async Task GetQueryValues()
        {
            var initPage = NavManager.ExtractQueryStringByKey<int?>("page");
            if (initPage != null)
            {
                var page = (int)initPage;
                if (page < 1)
                    _activePage = 0;
                else
                    _activePage = page - 1;
            }
            else
                _activePage = 0;

            var initSortOption = NavManager.ExtractQueryStringByKey<string>("sortOption");
            if (initSortOption != null)
            {
                if (Enum.TryParse(initSortOption, out SortTagsEnum activeSorting))
                    _activeSorting = activeSorting;
                else
                    _activeSorting = SortTagsEnum.Popular;
            }
            else
                _activeSorting = SortTagsEnum.Popular;

            await GetSubTags();
        }

        /// <summary>
        ///     Metoda odpowiedzialna za pobranie subtagów z danej strony.
        /// </summary>
        private async Task GetSubTags()
        {
            _subTagPage = await TagRepository.GetSubTagsAsync(IdMainTag, _activePage, _activeSorting);
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
        private void ChangeSorting(SortTagsEnum newSortOption)
        {
            _activeSorting = newSortOption;

            var queryParams = new Dictionary<string, string>();
            var pageUri = NavManager.ExtractQueryStringByKey<int?>("page");
            if (pageUri != null)
                queryParams.Add("page", pageUri.ToString());
            queryParams.Add("sortOption", _activeSorting.ToString());

            NavManager.NavigateTo(NavManager.AddQueryParameters(queryParams), true);
        }

        /// <summary>
        ///     Metoda aktualizująca aktualną stronę przeglądanych tagów.
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        private void SetPage(int page)
        {
            _activePage = page;

            var queryParams = new Dictionary<string, string>
            {
                { "page", (_activePage + 1).ToString() }
            };

            var sortOptionUri = NavManager.ExtractQueryStringByKey<string>("sortOption");
            if (sortOptionUri != null)
                queryParams.Add("sortOption", sortOptionUri);
            NavManager.NavigateTo(NavManager.AddQueryParameters(queryParams), true);
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