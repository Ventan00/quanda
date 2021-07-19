using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Quanda.Server.Repositories.Interfaces;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.Enums;

namespace Quanda.Server.Controllers
{
    /// <summary>
    ///     Kontroler który obsługuje końcówki związane z kategoriami
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        /// <summary>
        ///     Repozytorium wykonujące akcje na kategoriach
        /// </summary>
        private readonly ICategoryRepository _repository;

        /// <summary>
        ///     Konstruktor towrzący kontroler
        /// </summary>
        /// <param name="repository">Wstrzykiwane repozytorium</param>
        public CategoriesController(ICategoryRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        ///     Końcówka która zwraca kategorie danego pytania
        /// </summary>
        /// <param name="idQuestion">ID pytania w BD</param>
        /// <returns>List(CategoriesResponseDTO)</returns>
        [HttpGet("question/{idQuestion:int}")]
        public async Task<IActionResult> GetCategoriesOfQuestion(int idQuestion)
        {
            var result = await _repository.GetCategoriesOfQuestionAsync(idQuestion);
            return result != null ? Ok(result) : NotFound();
        }

        /// <summary>
        ///     Końcówka zwracająca wszystkie kategorie
        /// </summary>
        /// <returns>List(CategoriesResponseDTO)</returns>
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            return Ok(await _repository.GetCategoriesAsync());
        }

        /// <summary>
        ///     Końcówka która pozwala zmodyfikować kategorię
        /// </summary>
        /// <param name="category">Obiekt DTO opisujący zmiany</param>
        /// <param name="IdCategory">ID kategori w BD</param>
        /// <returns>IActionResult</returns>
        [HttpPut("{IdCategory:int}")]
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryDTO category,
            [FromRoute] int IdCategory)
        {
            return await _repository.UpdateCategoryAsync(category, IdCategory) switch
            {
                CategoryResultEnum.CATEGORY_NOT_FOUND => NotFound(),
                CategoryResultEnum.CATEGORY_UPDATED => Ok(),
                CategoryResultEnum.CATEGORY_DATABASE_ERROR => BadRequest(),
                _ => throw new ArgumentException()
            };
        }

        /// <summary>
        ///     Końcówka pozwalająca dodać kategorię do BD
        /// </summary>
        /// <param name="category">Obiekt DTO opisujący dodawaną kategorię</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] AddCategoryDTO category)
        {
            return await _repository.AddCategoryAsync(category) switch
            {
                CategoryResultEnum.CATEGORY_CREATED => Ok(),
                CategoryResultEnum.CATEGORY_DATABASE_ERROR => BadRequest(),
                _ => throw new ArgumentException()
            };
        }

        /// <summary>
        ///     Końcówka która pozwala usunąć kategorię z BD
        /// </summary>
        /// <param name="IdCategory">ID Kategorii w BD</param>
        /// <returns>IActionResult</returns>
        [HttpDelete("{IdCategory:int}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] int IdCategory)
        {
            return await _repository.DeleteCategoryAsync(IdCategory) switch
            {
                CategoryResultEnum.CATEGORY_NOT_FOUND => NotFound(),
                CategoryResultEnum.CATEGORY_DELETED => Ok(),
                CategoryResultEnum.CATEGORY_DATABASE_ERROR => BadRequest(),
                _ => throw new ArgumentException()
            };
        }
    }
}