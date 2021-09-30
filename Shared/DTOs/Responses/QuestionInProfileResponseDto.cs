using System;
using System.Collections.Generic;

namespace Quanda.Shared.DTOs.Responses
{
    public class QuestionInProfileResponseDto
    {
        public int IdQuestion { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Views { get; set; }
        public int Answers { get; set; }

        public IEnumerable<TagResponseDTO> Tags { get; set; }
    }
}
