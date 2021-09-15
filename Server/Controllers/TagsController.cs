using Microsoft.AspNetCore.Mvc;
using Quanda.Server.Repositories.Interfaces;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.Enums;
using System;
using System.Threading.Tasks;

namespace Quanda.Server.Controllers
{
    /// <summary>
    ///     Kontroler który obsługuje końcówki związane z tagami
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        /// <summary>
        ///     Repozytorium wykonujące akcje na tagach
        /// </summary>
        private readonly ITagRepository _repository;

        /// <summary>
        ///     Konstruktor tworzący kontroler
        /// </summary>
        /// <param name="repository">Wstrzykiwane repozytorium</param>
        public TagsController(ITagRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        ///     Końcówka która zwraca tag danego pytania
        /// </summary>
        /// <param name="idQuestion">ID pytania w BD</param>
        /// <returns>List(TagResponseDTO)</returns>
        [HttpGet("question/{idQuestion:int}")]
        public async Task<IActionResult> GetTagsOfQuestion(int idQuestion)
        {
            var result = await _repository.GetTagsOfQuestionAsync(idQuestion);
            return result != null ? Ok(result) : NotFound();
        }

        /// <summary>
        ///     Końcówka, która zwraca tagi z podanej strony.
        /// </summary>
        /// <param name="page">Aktualna strona przeglądanych tagów.</param>
        /// <returns>TagsPageResponseDTO</returns>
        [HttpGet]
        public async Task<IActionResult> GetTags([FromQuery] int page)
        {
            return Ok(await _repository.GetTagsAsync(page));
        }

        /// <summary>
        ///     Końcówka, która zwraca subtagi z podanej strony.
        /// </summary>
        /// <param name="idMainTag">Id nadrzędnego tagu.</param>
        /// <param name="page">Aktualna strona przeglądanych subtagów.</param>
        /// <returns>SubTagsPageResponseDTO</returns>
        [HttpGet("{idMainTag:int}")]
        public async Task<IActionResult> GetSubTags(int idMainTag, [FromQuery] int page)
        {
            return Ok(await _repository.GetSubTagsAsync(idMainTag, page));
        }

        /// <summary>
        ///     Końcówka która pozwala zmodyfikować tag
        /// </summary>
        /// <param name="tag">Obiekt DTO opisujący zmiany</param>
        /// <param name="IdTag">Id tagu w BD</param>
        /// <returns>IActionResult</returns>
        [HttpPut("{IdTag:int}")]
        public async Task<IActionResult> UpdateTag([FromBody] UpdateTagDTO tag,
            [FromRoute] int IdTag)
        {
            return await _repository.UpdateTagAsync(tag, IdTag) switch
            {
                TagResultEnum.TAG_NOT_FOUND => NotFound(),
                TagResultEnum.TAG_UPDATED => Ok(),
                TagResultEnum.TAG_DATABASE_ERROR => BadRequest(),
                _ => throw new ArgumentException()
            };
        }

        /// <summary>
        ///     Końcówka pozwalająca dodać tag do BD
        /// </summary>
        /// <param name="tag">Obiekt DTO opisujący dodawany tag</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> AddTag([FromBody] AddTagDTO tag)
        {
            return await _repository.AddTagAsync(tag) switch
            {
                TagResultEnum.TAG_CREATED => Ok(),
                TagResultEnum.TAG_DATABASE_ERROR => BadRequest(),
                _ => throw new ArgumentException()
            };
        }

        /// <summary>
        ///     Końcówka która pozwala usunąć tag z BD
        /// </summary>
        /// <param name="idTag">Id tagu w BD</param>
        /// <returns>IActionResult</returns>
        [HttpDelete("{idTag:int}")]
        public async Task<IActionResult> DeleteTag([FromRoute] int idTag)
        {
            return await _repository.DeleteTagAsync(idTag) switch
            {
                TagResultEnum.TAG_NOT_FOUND => NotFound(),
                TagResultEnum.TAG_DELETED => Ok(),
                TagResultEnum.TAG_DATABASE_ERROR => BadRequest(),
                _ => throw new ArgumentException()
            };
        }

    }
}