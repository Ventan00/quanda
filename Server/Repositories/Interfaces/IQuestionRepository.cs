using System.Collections.Generic;
using System.Threading.Tasks;
using Quanda.Server.Utils;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.DTOs.Responses;
using Quanda.Shared.Enums;
using Quanda.Shared.Models;

namespace Quanda.Server.Repositories.Interfaces
{
    /// <summary>
    ///     Repozytorium odpowiedzialne za pytania
    /// </summary>
    public interface IQuestionRepository
    {
        /// <summary>
        ///     Zwraca listę o długości QUESTIONS_PAGINATION_TAKE_SKIP
        ///     z klasy Config w folderze shared pytań według odpowiednich opcji sortowania
        /// </summary>
        /// <param name="skip">
        ///     Ilość pytań które system ma pominąć podczas pobierania
        ///     ich z bazy danych
        /// </param>
        /// <param name="sortOption">
        ///     Parametr opisujący sposób sortowania pobranych pytań.
        /// </param>
        /// <param name="categories">
        ///     Opcjonalna lista kategorii służąca do filtracji pytań
        ///     po przez przypisane do nich kategorie
        /// </param>
        /// <returns>List(GetQuestionsDTO)</returns>
        Task<List<GetQuestionsDTO>> GetQuestions(int skip, SortOptionEnum sortOption, List<int>? categories);

        /// <summary>
        ///     Końcówka która zwraca Question o podanym questionId
        /// </summary>
        /// <param name="questionId">ID pytania w BD</param>
        /// <returns>Question</returns>
        Task<Question> GetQuestion(int questionId);

        /// <summary>
        ///     Dodaje Question do BD
        /// </summary>
        /// <param name="question">Obiekt DTO służący do dodania Question do DB</param>
        /// <returns>QuestionStatusResult</returns>
        Task<QuestionStatusResult> AddQuestion(AddQuestionDTO question);

        /// <summary>
        ///     Końcówka pozwalająca zmodyfikować Question w BD
        /// </summary>
        /// <param name="questionId">ID pytania w BD</param>
        /// <param name="question">Obiekt DTO służący do dodania zmodyfikowanego Question do DB</param>
        /// <returns>QuestionStatusResult</returns>
        Task<QuestionStatusResult> UpdateQuestion(int questionId, UpdateQuestionDTO question);

        /// <summary>
        ///     Usuwa Question z BD
        /// </summary>
        /// <param name="questionId">ID pytania w BD</param>
        /// <returns>QuestionStatusResult</returns>
        Task<QuestionStatusResult> RemoveQuestion(int questionId);

        /// <summary>
        ///     Zmienia stan pytania w stan konieczności
        ///     zweryfikowania przez moderatora bądź koniec takiej potrzeby
        /// </summary>
        /// <param name="questionId">ID pytania w BD</param>
        /// <param name="value">
        ///     Wartość jaką ma ustawić końcówka stan pytania.
        ///     true = trzeba sprawdzić, false = nie trzeba sprawdzać
        /// </param>
        /// <returns>QuestionStatusResult</returns>
        Task<QuestionStatusResult> SetToCheck(int questionId, bool value);

        /// <summary>
        ///     Ustawia stan pytania na zakończony
        /// </summary>
        /// <param name="questionId">ID pytania w BD</param>
        /// <returns>QuestionStatusResult</returns>
        Task<QuestionStatusResult> SetFinished(int questionId);

        /// <summary>
        ///     Zwraca ilość Question z podanymi Categories
        /// </summary>
        /// <param name="category">Lista kategorii do filtrowania pytań</param>
        /// <returns>int</returns>
        Task<int> GetAmountOfQuestions(List<int> category);
    }
}