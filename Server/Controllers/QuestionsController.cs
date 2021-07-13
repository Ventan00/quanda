﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quanda.Server.Repositories.Interfaces;
using Quanda.Server.Utils;
using Quanda.Shared.Models;
using Quanda.Shared.DTOs.Requests;
using Quanda.Shared.Enums;

namespace Quanda.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionRepository _repository;

        public QuestionsController(IQuestionRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetQuestions([FromQuery] int skip, [FromQuery] string sortOption, [FromQuery] List<int> category=null)
        {
            if (category == null)
                category = new List<int>();
            var questions = await _repository.GetQuestions(skip,
                (SortOptionEnum) Enum.Parse(typeof(SortOptionEnum), sortOption), category);
            questions.Reverse();
            return Ok(questions);
        }

        [HttpGet("{QuestionID}")]
        public async Task<IActionResult> GetQuestion([FromRoute]int QuestionID)
        {
            var question = await _repository.GetQuestion(QuestionID);
            return question!=null ? Ok(question) : NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> AddQuestion([FromBody] AddQuestionDTO question)
        {
            return await _repository.AddQuestion(question) == QuestionStatusResult.QUESTION_ADDED ? Ok() : BadRequest();
        }
        [HttpPut("{QuestionID}")]
        public async Task<IActionResult> UpdateQuestion([FromRoute] int QuestionID, [FromBody] UpdateQuestionDTO question)
        {
            var statusCode = await _repository.UpdateQuestion(QuestionID, question);
            if (statusCode == QuestionStatusResult.QUESTION_UPDATED) return Ok();
            else if (statusCode == QuestionStatusResult.QUESTION_NOT_FOUND) return NotFound();
            else return BadRequest();
        }
        [HttpPut("{QuestionID:int}/{CheckValue:bool}")]
        public async Task<IActionResult> SetToCheck([FromRoute] int QuestionID, [FromRoute]bool CheckValue)
        {
            var statusCode = await _repository.SetToCheck(QuestionID, CheckValue);
            if (statusCode == QuestionStatusResult.QUESTION_CHANGED_TOCHECK_STATUS) return Ok();
            else if (statusCode == QuestionStatusResult.QUESTION_NOT_FOUND) return NotFound();
            else return BadRequest();
        }
        [HttpPut("{QuestionID:int}")]
        public async Task<IActionResult> SetFinished([FromRoute] int QuestionID)
        {
            var statusCode = await _repository.SetFinished(QuestionID);
            if (statusCode == QuestionStatusResult.QUESTION_SET_TO_FINISHED) return Ok();
            else if (statusCode == QuestionStatusResult.QUESTION_NOT_FOUND) return NotFound();
            else return BadRequest();
        }
        [HttpDelete("{QuestionID:int}")]
        public async Task<IActionResult> DeleteQuestion([FromRoute] int QuestionID)
        {
            var statusCode = await _repository.RemoveQuestion(QuestionID);
            if (statusCode == QuestionStatusResult.QUESTION_DELETED) return Ok();
            else if (statusCode == QuestionStatusResult.QUESTION_NOT_FOUND) return NotFound();
            else return BadRequest();
        }

        [HttpGet("count")]
        public async Task<IActionResult> GetAmountOfQuestions([FromQuery] List<int> category = null)
        {
            if (category == null)
                category = new List<int>();
            return Ok(await _repository.GetAmountOfQuestions(category));
        }

    }
}
