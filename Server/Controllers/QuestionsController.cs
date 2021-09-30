using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Quanda.Server.Extensions;
using Quanda.Server.Repositories.Interfaces;
using Quanda.Server.Utils;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.Enums;

namespace Quanda.Server.Controllers
{
    /// <summary>
    ///     Kontroler który obsługuje końcówki związane z pytaniami
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        /// <summary>
        ///     Repozytorium wykonujące akcje na pytaniach
        /// </summary>
        private readonly IQuestionRepository _repository;

        /// <summary>
        ///     Konstruktor towrzący kontroler
        /// </summary>
        /// <param name="repository">Wstrzykiwane repozytorium</param>
        public QuestionsController(IQuestionRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        ///     Końcówka która zwraca listę o długości QUESTIONS_PAGINATION_TAKE_SKIP
        ///     z klasy Config w folderze shared pytań według odpowiednich opcji sortowania
        /// </summary>
        /// <param name="skip">
        ///     Ilość pytań które system ma pominąć podczas pobierania
        ///     ich z bazy danych
        /// </param>
        /// <param name="sortOption">
        ///     Parametr opisujący sposób sortowania pobranych pytań.
        ///     Ma przełożenie na SortOptionEnum
        /// </param>
        /// <param name="tags">
        ///     Opcjonalna lista tagów służąca do filtracji pytań
        ///     po przez przypisane do nich tagi
        /// </param>
        /// <returns>GetQuestionsDTO</returns>
        [HttpGet]
        public async Task<IActionResult> GetQuestions([FromQuery] int skip, [FromQuery] string sortOption,
            [FromQuery] List<int> tag = null)
        {
            if (tag == null)
                tag = new List<int>();
            var questions = await _repository.GetQuestions(skip,
                (SortOptionEnum)Enum.Parse(typeof(SortOptionEnum), sortOption), tag);
            return Ok(questions);
        }

        /// <summary>
        ///     Końcówka która zwraca pytanie o podanym idQuestion
        /// </summary>
        /// <param name="QuestionID">Id pytania w BD</param>
        /// <returns>QuestionResponseDTO</returns>
        [HttpGet("{idQuestion}")]
        public async Task<IActionResult> GetQuestion([FromRoute] int idQuestion)
        {
            var requestIdUser = HttpContext.User.GetId();
            var question = await _repository.GetQuestion(idQuestion, requestIdUser);
            return question != null ? Ok(question) : NotFound();
        }

        /// <summary>
        ///     Końcówka pozwalająca dodać Question do BD
        /// </summary>
        /// <param name="question">Obiekt DTO służący do przesłania Question do DB</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> AddQuestion([FromBody] AddQuestionDTO question)
        {
            return await _repository.AddQuestion(question) == QuestionStatusResult.QUESTION_ADDED ? Ok() : BadRequest();
        }

        /// <summary>
        ///     Końcówka pozwalająca zmodyfikować Question w BD
        /// </summary>
        /// <param name="QuestionID">ID pytania w BD</param>
        /// <param name="question">Obiekt DTO służący do przesłania zmodyfikowanego Question do DB</param>
        /// <returns>IActionResult</returns>
        [HttpPut("{QuestionID}")]
        public async Task<IActionResult> UpdateQuestion([FromRoute] int QuestionID,
            [FromBody] UpdateQuestionDTO question)
        {
            var statusCode = await _repository.UpdateQuestion(QuestionID, question);
            if (statusCode == QuestionStatusResult.QUESTION_UPDATED) return Ok();
            if (statusCode == QuestionStatusResult.QUESTION_NOT_FOUND) return NotFound();
            return BadRequest();
        }

        /// <summary>
        ///     Końcówka która zmienia stan pytania w stan konieczności
        ///     zweryfikowania przez moderatora bądź koniec takiej potrzeby
        /// </summary>
        /// <param name="QuestionID">ID pytania w BD</param>
        /// <param name="CheckValue">
        ///     Wartość jaką ma ustawić końcówka stan pytania.
        ///     true = trzeba sprawdzić, false = nie trzeba sprawdzać
        /// </param>
        /// <returns>IActionResult</returns>
        [HttpPut("{QuestionID:int}/{CheckValue:bool}")]
        public async Task<IActionResult> SetToCheck([FromRoute] int QuestionID, [FromRoute] bool CheckValue)
        {
            var statusCode = await _repository.SetToCheck(QuestionID, CheckValue);
            if (statusCode == QuestionStatusResult.QUESTION_CHANGED_TOCHECK_STATUS) return Ok();
            if (statusCode == QuestionStatusResult.QUESTION_NOT_FOUND) return NotFound();
            return BadRequest();
        }

        /// <summary>
        ///     Końcówka która ustawia stan pytania na zakończony
        /// </summary>
        /// <param name="QuestionID">ID pytania w BD</param>
        /// <returns>IActionResult</returns>
        [HttpPut("{QuestionID:int}")]
        public async Task<IActionResult> SetFinished([FromRoute] int QuestionID)
        {
            var statusCode = await _repository.SetFinished(QuestionID);
            if (statusCode == QuestionStatusResult.QUESTION_SET_TO_FINISHED) return Ok();
            if (statusCode == QuestionStatusResult.QUESTION_NOT_FOUND) return NotFound();
            return BadRequest();
        }

        /// <summary>
        ///     Usuwa Question z BD
        /// </summary>
        /// <param name="QuestionID">ID pytania w BD</param>
        /// <returns>IActionResult</returns>
        [HttpDelete("{QuestionID:int}")]
        public async Task<IActionResult> DeleteQuestion([FromRoute] int QuestionID)
        {
            var statusCode = await _repository.RemoveQuestion(QuestionID);
            if (statusCode == QuestionStatusResult.QUESTION_DELETED) return Ok();
            if (statusCode == QuestionStatusResult.QUESTION_NOT_FOUND) return NotFound();
            return BadRequest();
        }

        /// <summary>
        ///     Zwraca ilość Question z podanymi Categories
        /// </summary>
        /// <param name="tags">Lista tagów do filtrowania pytań</param>
        /// <returns>int</returns>
        [HttpGet("count")]
        public async Task<IActionResult> GetAmountOfQuestions([FromQuery] List<int> tags = null)
        {
            if (tags == null)
                tags = new List<int>();
            return Ok(await _repository.GetAmountOfQuestions(tags));
        }

        /// <summary>
        ///    API endpoint for getting questions created by given user
        /// </summary>
        /// <param name="idUser">Id of user whose questions should be returned</param>
        /// <param name="skip">Amount of questions that are already loaded and should be skipped</param>
        /// <returns>
        ///     NotFound,
        ///     Ok => GetProfileQuestionsResponseDto
        /// </returns>
        [Authorize]
        [HttpGet("users/{idUser:int}")]
        public async Task<IActionResult> GetUserQuestions(
            [FromRoute] int idUser, [FromQuery] int skip)
        {
            var profileQuestions = await _repository.GetUserProfileQuestionsAsync(idUser, skip);
            if (profileQuestions is null)
                return NotFound();

            return Ok(profileQuestions);
        }
    }
}