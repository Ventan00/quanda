using System;
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
        ///     tagi pytań
        /// </summary>
        /// <param name="page">Strona którą ma zwrócić repozytorium</param>
        /// <param name="sortingBy">Typ sortowania pytań</param>
        /// <param name="tags">Wybrane tagi pytań</param>
        /// <returns>GetQuestionsDTO</returns>
        public Task<GetQuestionsDTO> GetQuestions(int page, SortOptionEnum sortingBy, List<int> tags);

        /// <summary>
        ///     Zwraca liczbę pytań w które są danych kategorii
        /// </summary>
        /// <param name="selectedTags">Lista wybranych tagów pytań</param>
        /// <returns>int</returns>
        public Task<int> GetQuestionsAmount(List<int> selectedTags);

        /// <summary>
        ///     Zwraca pytanie z listą odpowiedzi z przedziału z zakresu ANSWERS_PAGE_SIZE.
        /// </summary>
        /// <param name="idQuestion">Id pytania</param>
        /// <returns>GetQuestionsDTO</returns>
        public Task<QuestionResponseDTO> GetQuestion(int idQuestion);

        /// <summary>
        ///     Sending API Request in order to get questions created by given user
        /// </summary>
        /// <param name="idUser">Id of user whose questions you want to receive</param>
        /// <param name="skip">Amount of questions that are already loaded and should be skipped</param>
        /// <returns>GetProfileQuestionsResponseDto</returns>
        public Task<GetProfileQuestionsResponseDto> GetUserQuestionsAsync(int idUser, int skip);
    }
}