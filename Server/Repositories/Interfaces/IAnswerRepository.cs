using Quanda.Server.Utils;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.DTOs.Responses;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Quanda.Server.Repositories.Interfaces
{
    public interface IAnswerRepository
    {

        /// <summary>
        /// Metoda zwraca listę odpowiedzi na pytanie z podanego przedziału.
        /// </summary>
        /// <param name="idQuestion">Id pytania, do którego odnoszą się odpowiedzi.</param>
        /// <param name="idUserLogged">Id użytkownika przeglądającego pytanie. Używane w celu wyznaczenia informacji o głosach oddanych na odpowiedzi.</param>
        /// <param name="answersParams">Zawiera parametry dotyczące ilości elementów na stronie oraz początkowy indeks do pobrania nowej listy odpowiedzi.</param>
        /// <returns>Lista odpowiedzi z podanego przedziału.</returns>
        Task<List<AnswerResponseDTO>> GetAnswersAsync(int idQuestion, int idUserLogged, AnswersPageDTO answersParams);

        /// <summary>
        /// Metoda zwraca konkretną odpowiedź.
        /// </summary>
        /// <param name="idAnswer">Id odpowiedzi pobieranej.</param>
        /// <param name="idUserLogged">Id użytkownika przeglądającego pytanie. Używane w celu wyznaczenia informacji o głosach oddanych na odpowiedzi.</param>
        /// <returns>Konkretna odpowiedź.</returns>
        Task<AnswerResponseDTO> GetAnswerAsync(int idAnswer, int idUserLogged);

        /// <summary>
        /// Metoda odpowiedzialna za dodanie odpowiedzi do bazy. Przed dodaniem sprawdzone zostaje: czy pytanie oraz konto użytkownika nadal istnieje. W przypadku usunięcia odpowiedzi do którego odnosiła się odpowiedź, odpowiedź staję się z kategorii główną.
        /// </summary>
        /// <param name="answerDTO"></param>
        /// <param name="idUserLogged">Id użytkownika przeglądającego pytanie. Używane w celu sprawdzeniu czy użytkownik nadal istnieje w bazie.</param>
        /// <returns>Informacja o sukcesie akcji dodania odpowiedzi.</returns>
        Task<AnswerResult> AddAnswerAsync(AddAnswerDTO answerDTO, int idUserLogged);

        /// <summary>
        /// Metoda odpowiedzialna za aktualizowanie treści odpowiedzi.
        /// </summary>
        /// <param name="idAnswer">Id odpowiedzi, której tekst zostanie zaktualizowany.</param>
        /// <param name="answerDTO">Nowy tekst odpowiedzi.</param>
        /// <returns>Informacja o sukcesie akcji edycji odpowiedzi.</returns>
        Task<AnswerResult> UpdateAnswerAsync(int idAnswer, UpdateAnswerDTO answerDTO);

        /// <summary>
        /// Metoda odpowiedzialna za usunięcie odpowiedzi.
        /// </summary>
        /// <param name="idAnswer">Id odpowiedzi do usunięcia.</param>
        /// <param name="idUserLogged">Id użytkownika przeglądającego pytanie. Używane w celu zabezpieczenia przed usunięciem odpowiedzi, przed użytkownika nie będącego jego właścicielem.</param>
        /// <returns>Informacja o sukcesie akcji usunięcia odpowiedzi.</returns>
        Task<AnswerResult> DeleteAnswerAsync(int idAnswer, int idUserLogged);

        /// <summary>
        /// Metoda odpowiedzialna za aktualizacje rankingu odpowiedzi. W bazie są trzymane jedynie rekordy ocenionych pytań. Kiedy ocena odpowiedzi zmienia się na pozytywną lub negatywną powstaje rekord w bazie. W odwrotnym przypadku kiedy ocena odpowiedzi użytkownika wraca do stanu neuatralnego rekord z bazy zostaje usunięty.
        /// </summary>
        /// <param name="idAnswer">Id odpowiedzi do aktualizacji oceny.</param>
        /// <param name="idUserLogged">Id użytkownika przeglądającego pytanie.</param>
        /// <param name="updateRatingAnswer">Zawiera nową ocenę odpowiedzi.</param>
        /// <returns>Informacja o sukcesie akcji edycji oceny odpowiedzi.</returns>
        Task<AnswerResult> UpdateRatingAnswerAsync(int idAnswer, int idUserLogged, UpdateRatingAnswerDTO updateRatingAnswer);

    }
}
