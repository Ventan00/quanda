using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quanda.Server.Repositories.Interfaces;

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
            var result = await _repository.GetCategoriesOfQuestinAsync(idQuestion);
            return result != null ? Ok(result) : NotFound();
        }
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            return Ok(await _repository.GetCategoriesAsync());
        }

        [HttpPut] 
        public async Task<IActionResult> UpdateCategory()
        {
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> AddCategory()
        {
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteCategory()
        {
            return Ok();
        }

    }
}
