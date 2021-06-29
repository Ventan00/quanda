using Microsoft.AspNetCore.Mvc;
using Quanda.Server.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Quanda.Server.Controllers
{
    [Route("api/answers")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        private readonly IAnswerRepository _repository;

        public AnswersController(IAnswerRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{idQuestion}")]
        public async Task<IActionResult> GetAnswers(int idQuestion)
        {
            var result = await _repository.GetAnwsersAsync(idQuestion);
            return Ok(result);
        }
    }
}
