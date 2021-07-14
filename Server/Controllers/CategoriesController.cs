using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quanda.Server.Repositories.Interfaces;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.Enums;

namespace Quanda.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _repository;

        public CategoriesController(ICategoryRepository repository)
        {
            _repository = repository;
        }
        [HttpGet("question/{idQuestion:int}")]
        public async Task<IActionResult> GetCategoriesOfQuestion(int idQuestion)
        {
            var result = await _repository.GetCategoriesOfQuestionAsync(idQuestion);
            return result != null ? Ok(result) : NotFound();
        }
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            return Ok(await _repository.GetCategoriesAsync());
        }

        [HttpPut("{IdCategory:int}")] 
        public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryDTO category,[FromRoute] int IdCategory )
        {
            return await _repository.UpdateCategoryAsync(category, IdCategory) switch
            {
                CategoryResultEnum.CATEGORY_NOT_FOUND => NotFound(),
                CategoryResultEnum.CATEGORY_UPDATED => Ok(),
                CategoryResultEnum.CATEGORY_DATABASE_ERROR => BadRequest(),
                _ => throw new ArgumentException()
            };
            
        }
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
