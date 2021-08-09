using Microsoft.AspNetCore.Mvc;
using Quanda.Server.Extensions;
using Quanda.Server.Repositories.Interfaces;
using Quanda.Server.Utils;
using Quanda.Shared.DTOs.Requests;
using System.Threading.Tasks;

namespace Quanda.Server.Controllers
{
    /// <summary>
    ///     Kontroler obsługujący końcówki związane z odpowiedziami.
    /// </summary>
    [Route("api/answers")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        /// <summary>
        ///     Repozytorium wykonujące akcje na odpowiedziach.
        /// </summary>
        private readonly IAnswerRepository _repository;

        /// <summary>
        ///     Konstruktor tworzący kontroler.
        /// </summary>
        /// <param name="repository">Wstrzykiwane repozytorium</param>
        public AnswersController(IAnswerRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        ///     Końcówka zwracająca listę odpowiedzi z konkretnego pytania z podanego przedziału.
        /// </summary>
        /// <param name="idQuestion">Id pytania do którego odnoszą się odpowiedzi.</param>
        /// <param name="answersPage">Zawiera informacje z jakiego zakresu pobrać odpowiedzi.</param>
        /// <returns>Lista odpowiedzi.</returns>
        [HttpGet("{idQuestion}")]
        public async Task<IActionResult> GetAnswers(int idQuestion, [FromQuery] AnswersPageDTO answersPage)
        {
            var requestIdUser = HttpContext.Request.GetUserId();
            var result = await _repository.GetAnswersAsync(idQuestion, requestIdUser, answersPage);
            return Ok(result);
        }

        /// <summary>
        ///     Końcówka zwracająca listę pododpowiedzi.
        /// </summary>
        /// <param name="idAnswer">Id głownej odpowiedzi.</param>
        /// <returns>Lista pododpowiedzi.</returns>
        [HttpGet("{idAnswer}/children")]
        public async Task<IActionResult> GetAnswerChildrenAsync(int idAnswer)
        {
            var requestIdUser = HttpContext.Request.GetUserId();
            var result = await _repository.GetAnswerChildrenAsync(idAnswer, requestIdUser);
            return Ok(result);
        }

        /// <summary>
        ///     Końcówka odpowiedzialna za dodanie odpowiedzi do bazy.
        /// </summary>
        /// <param name="answerDTO">Zawiera informacje dotyczące nowej odpowiedzi.</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> AddAnswer([FromBody] AddAnswerDTO answerDTO)
        {
            var requestIdUser = HttpContext.Request.GetUserId();
            var result = await _repository.AddAnswerAsync(answerDTO, requestIdUser);
            if (result == AnswerResult.QUESTION_DELETED || result == AnswerResult.USER_DELETED)
                return BadRequest(result.ToString());
            else if (result == AnswerResult.ADD_DB_ERROR)
                return StatusCode(500, result.ToString());
            return NoContent();
        }

        /// <summary>
        ///     Końcówka odpowiedzialna za zaktualizawanie odpowiedzi.
        /// </summary>
        /// <param name="idAnswer">Id odpowiedzi do aktualizacji.</param>
        /// <param name="answerDTO">Zawiera nową treść odpowiedzi.</param>
        /// <returns>IActionResult</returns>
        [HttpPut("{idAnswer}")]
        public async Task<IActionResult> UpdateAnswer(int idAnswer, [FromBody] UpdateAnswerDTO answerDTO)
        {
            var result = await _repository.UpdateAnswerAsync(idAnswer, answerDTO);
            if (result == AnswerResult.ANSWER_DELETED)
                return NotFound(result.ToString());
            else if (result == AnswerResult.UPDATE_DB_ERROR)
                return StatusCode(500, result.ToString());
            return NoContent();
        }

        /// <summary>
        ///     Końcówka odpowiedzialna usunięcie odpowiedzi.
        /// </summary>
        /// <param name="idAnswer">Id odpowiedzi do usunięcia.</param>
        /// <returns>IActionResult</returns>
        [HttpDelete("{idAnswer}")]
        public async Task<IActionResult> DeleteAnswer(int idAnswer)
        {
            var requestIdUser = HttpContext.Request.GetUserId();
            var result = await _repository.DeleteAnswerAsync(idAnswer);
            if (result == AnswerResult.ANSWER_DELETED)
                return NotFound(result.ToString());
            else if (result == AnswerResult.DELETE_DB_ERROR)
                return StatusCode(500, result.ToString());
            return NoContent();
        }

        /// <summary>
        ///     Koncówka odpowiedzialna za zaktualizowanie ratingu odpowiedzi.
        /// </summary>
        /// <param name="idAnswer">Id odpowiedzi do aktualizacji ratingu.</param>
        /// <param name="updateRatingAnswer">Zawiera informacja o nowym ratingu.</param>
        /// <returns>IActionResult</returns>
        [HttpPost("{idAnswer}/rating")]
        public async Task<IActionResult> UpdateRatingAnswer(int idAnswer, [FromBody] UpdateRatingAnswerDTO updateRatingAnswer)
        {
            var requestIdUser = HttpContext.Request.GetUserId();
            var result = await _repository.UpdateRatingAnswerAsync(idAnswer, requestIdUser, updateRatingAnswer);
            if (result == AnswerResult.ANSWER_DELETED || result == AnswerResult.USER_DELETED)
                return BadRequest(result.ToString());
            else if (result == AnswerResult.OWNER_OF_ANSWER)
                return BadRequest(result.ToString());
            else if (result == AnswerResult.NOT_OWNER_OF_ANSWER)
                return Forbid(result.ToString());
            else if (result == AnswerResult.ADD_DB_ERROR || result == AnswerResult.DELETE_DB_ERROR)
                return StatusCode(500, result.ToString());

            return NoContent();
        }

    }
}
