using System.Collections.Generic;
using System.Threading.Tasks;
using Quanda.Shared.DTOs.Responses;
using Quanda.Shared.Enums;

namespace Quanda.Client.Repositories.Interfaces
{
    /// <summary>
    ///     Repozytorium odpowiedzialne za pytania
    /// </summary>
    public interface IQuestionsReposiotry
    {
        /// <summary>
        ///     Zwraca listę pytań na danej stronie używając sortowania według jednej z dostępnych opcji oraz uwzględnia wybrane
        ///     kategorie pytań
        /// </summary>
        /// <param name="page">Strona którą ma zwrócić repozytorium</param>
        /// <param name="sortingBy">Typ sortowania pytań</param>
        /// <param name="categories">Wybrane kategorie pytań</param>
        /// <returns>List(GetQuestionsDTO)</returns>
        public Task<List<GetQuestionsDTO>> GetQuestions(int page, SortOptionEnum sortingBy, List<int> categories);

        /// <summary>
        ///     Zwraca liczbę pytań w które są danych kategorii
        /// </summary>
        /// <param name="selectedCategories">Lista wybranych kategorii pytań</param>
        /// <returns>int</returns>
        public Task<int> GetQuestionsAmount(List<int> selectedCategories);
    }
}