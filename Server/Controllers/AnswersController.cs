using Microsoft.AspNetCore.Mvc;
using Quanda.Server.Repositories.Interfaces;
using Quanda.Server.Utils;
using Quanda.Shared.DTOs.Requests;
using System.Threading.Tasks;

namespace Quanda.Server.Controllers
{
    [Route("api/answers")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        private readonly IAnswerRepository _repository;
        private readonly int requestIdUser = 25; //future => Request.GetUser();

        public AnswersController(IAnswerRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{idQuestion}")]
        public async Task<IActionResult> GetAnswers(int idQuestion)
        {
            var result = await _repository.GetAnswersAsync(idQuestion, requestIdUser);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddAnswer([FromBody] AddAnswerDTO answerDTO)
        {
            var result = await _repository.AddAnswerAsync(answerDTO);
            if (result == AnswerResult.QUESTION_DELETED || result == AnswerResult.USER_DELETED)
                return BadRequest(result.ToString());
            else if (result == AnswerResult.ADD_DB_ERROR)
                return StatusCode(500, result.ToString());
            return NoContent();
        }

        [HttpPut("{idAnswer}")]
        public async Task<IActionResult> UpdateAnswer(int idAnswer,[FromBody] UpdateAnswerDTO answerDTO)
        {
            var result = await _repository.UpdateAnswerAsync(idAnswer, answerDTO);
            if (result == AnswerResult.ANSWER_DELETED)
                return NotFound(result.ToString());
            else if (result == AnswerResult.UPDATE_DB_ERROR)
                return StatusCode(500, result.ToString());
            return NoContent();
        }

        [HttpDelete("{idAnswer}")]
        public async Task<IActionResult> DeleteAnswer(int idAnswer)
        {
            var result = await _repository.DeleteAnswerAsync(idAnswer, requestIdUser);
            if (result == AnswerResult.ANSWER_DELETED)
                return NotFound(result.ToString());
            else if (result == AnswerResult.DELETE_DB_ERROR)
                return StatusCode(500, result.ToString());
            return NoContent();
        }

        [HttpPost("{idAnswer}/rating")]
        public async Task<IActionResult> UpdateRatingAnswer(int idAnswer, [FromBody] UpdateRatingAnswerDTO updateRatingAnswer)
        {
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
