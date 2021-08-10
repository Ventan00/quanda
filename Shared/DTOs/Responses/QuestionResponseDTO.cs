using System;
using System.Collections.Generic;

namespace Quanda.Shared.DTOs.Responses
{
    public class QuestionResponseDTO
    {
        public int IdQuestion { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public int Views { get; set; }
        public bool IsFinished { get; set; }
        public bool IsModified { get; set; }
        public int AnswersCount { get; set; }
        public UserResponseDTO User { get; set; }
        public List<TagResponseDTO> Tags { get; set; }
        public List<AnswerResponseDTO> Answers { get; set; }
    }
}
