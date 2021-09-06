using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Quanda.Client.Shared.RightMenu;
using Quanda.Shared.DTOs.Responses;
using Quanda.Shared.Enums;

namespace Quanda.Client.Components
{
    public partial class QuestionsComponent
    {

        /// <summary>
        ///     Zmienna która jest callbackiem main layoutu informująca czy i jakie prawe menu powinno być pokazane
        /// </summary>
        [CascadingParameter]
        protected EventCallback<RightMenuType> RightMenuTypeCallback { get; set; }

        /// <summary>
        ///     Zmienna która mówi czy pytania są ładowane z bazy danych
        /// </summary>
        bool isPending = true;

        /// <summary>
        ///     Zmienna opisująca ile jest pytań po zostasowaniu metod filtrowania
        /// </summary>
        private int questionAmount;

        /// <summary>
        ///     Aktualna opcja sortowania:
        /// </summary>
        private SortOptionEnum activeSorting;

        /// <summary>
        ///     Numer aktualnie przeglądanej strony
        /// </summary>
        public int activePage;

        /// <summary>
        ///     Lista standardowych pytań na danej stronie posortowana według wybranej opcji opisanej po przez zmienną activeSorting
        /// </summary>
        public List<QuestionGetQuestionsDTO> standardQuestions { get; set; }

        /// <summary>
        ///     Lista dodatkowych pytań na danej stronie posortowana według wybranej opcji opisanej po przez zmienną activeSorting 
        /// </summary>
        public List<QuestionGetQuestionsDTO> extraQuestions { get; set; }

        /// <summary>
        ///     Lista wybranych id tagów
        /// </summary>
        public List<int> selectedTags = new List<int>();

        protected override async Task OnInitializedAsync()
        {
            await RightMenuTypeCallback.InvokeAsync(RightMenuType.STANDARD);
            //selectedTags.Add(4); //mock, uncomment for testing
            //selectedTags.Add(2); //mock, uncomment for testing
            await GetQuestions();

        }

        /// <summary>
        ///     Funkcja która pobiera z BD listę pytań uwzględniając aktualną stronę, wybrane sortowanie oraz wybrane kategorie
        /// </summary>
        /// <returns></returns>
        private async Task GetQuestions()
        {
            var returnedQuestions = await _questionsReposiotry.GetQuestions(activePage, activeSorting, selectedTags);
            standardQuestions = returnedQuestions.StandardQuestions;
            extraQuestions = returnedQuestions.ExtraQuestions;
            questionAmount = returnedQuestions.QuestionsCount;
            isPending = false;
            StateHasChanged();
        }

        /// <summary>
        ///     Funkcja która zmienia aktualny tryb sortowania
        /// </summary>
        /// <param name="sortingOption">
        ///     Opisuje tryb sortowania
        /// </param>
        /// <returns></returns>
        private async Task SetSorting(SortOptionEnum sortingOption)
        {
            activeSorting = sortingOption;
            isPending = true;
            StateHasChanged();
            await GetQuestions();
        }

        /// <summary>
        ///     Funkcja dodaje Id tagu do listy wybranych tagów
        /// </summary>
        /// <param name="tagId">Id wybranego tagu</param>
        /// <returns></returns>
        private async Task TagSelected(int tagId)
        {
            activePage = 0;
            selectedTags.Add(tagId);
            isPending = true;
            StateHasChanged();
            await GetQuestions();
        }

        /// <summary>
        ///     Funkcja usuwa Id tagu listy wybranych tagów
        /// </summary>
        /// <param name="tagId">Id wybranego tagu</param>
        /// <returns></returns>
        private async Task TagDeselected(int tagId)
        {
            activePage = 0;
            selectedTags.Remove(tagId);
            isPending = true;
            StateHasChanged();
            await GetQuestions();
        }

        /// <summary>
        ///     Funkcja która zmienia aktualnie wyświetlaną stronę
        /// </summary>
        /// <param name="pageNumber">Numer wyświetlanej strony</param>
        /// <returns></returns>
        private async Task ChangeActivePage(int pageNumber)
        {
            activePage = pageNumber;
            isPending = true;
            StateHasChanged();
            await GetQuestions();
        }
    }
}
