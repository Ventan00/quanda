using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quanda.Client.Repositories.Interfaces
{
    /// <summary>
    ///     Repozytorium odpowiedzialne za odpowiedzi.
    /// </summary>
    public interface IAnswerRepository
    {
        /// <summary>
        ///     Zwraca listę odpowiedzi do konkretnego pytania.
        /// </summary>
        /// <param name="idQuestion"></param>
        /// <param name="productParams">Zakres odpowiedzi.</param>
        /// <returns>List<AnswerResponseDTO></returns>
        Task<List<AnswerResponseDTO>> GetAnswersAsync(int idQuestion, AnswersPageDTO productParams);

        /// <summary>
        ///     Zwraca listę pododpowiedzi.
        /// </summary>
        /// <param name="idAnswer">Id głównej odpowiedzi.</param>
        /// <returns>List<AnswerResponseDTO></returns>
        Task<List<AnswerResponseDTO>> GetAnswerChildrenAsync(int idAnswer);

        /// <summary>
        ///     Metoda odpowiedzialna za wysłanie żądania dodania odpowiedzi.Zwraca sukces dodania do bazy wraz ewentualną trecią komunikatu odpowiedzi. 
        /// </summary>
        /// <param name="text">Treść nowo dodanej odpowiedzi.</param>
        /// <param name="idQuestion">Id pytania do którego odnosi się odpowiedź.</param>
        /// <param name="idRootAnswer">Id odpowiedzi nadrzędnego.</param>
        /// <returns>Tuple<bool, string></returns>
        Task<Tuple<bool, string>> AddAnswer(string text, int idQuestion, int idRootAnswer);

        /// <summary>
        ///     Metoda odpowiedzialna za wysłanie żądania zaktualizowania odpowiedzi.Zwraca sukces aktulizacji w bazie wraz ewentualną trecią komunikatu odpowiedzi. 
        /// </summary>
        /// <param name="idAnswer">Id odpowiedzi do aktualizacji.</param>
        /// <param name="text">Treść zaktualizowanej odpowiedzi.</param>
        /// <returns>Tuple<bool, string></returns>
        Task<Tuple<bool, string>> UpdateAnswer(int idAnswer, string text);

        /// <summary>
        ///     Metoda odpowiedzialna za wysłanie żądania usunięcia odpowiedzi.Zwraca sukces usunięcia z bazy wraz ewentualną trecią komunikatu odpowiedzi. 
        /// </summary>
        /// <param name="idAnswer">Id odpowiedzi do usunięcia.</param>
        /// <returns>Tuple<bool, string></returns>
        Task<Tuple<bool, string>> DeleteAnswer(int idAnswer);

        /// <summary>
        ///     Metoda odpowiedzialna za wysłanie żądania zaktulizowania ratingu odpowiedzi.Zwraca sukces zaktualizowania z bazy wraz ewentualną trecią komunikatu odpowiedzi. 
        /// </summary>
        /// <param name="idAnswer">Id odpowiedzi do zaktulizowania ratingu.</param>
        /// <param name="rating">Nowy rating odpowiedzi.</param>
        /// <returns>Tuple<bool, string></returns>
        Task<Tuple<bool, string>> UpdateRatingAnswerAsync(int idAnswer, int rating);

    }
}
