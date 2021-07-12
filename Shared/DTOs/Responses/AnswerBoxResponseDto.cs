using System.Collections.Generic;

namespace Quanda.Shared.DTOs.Responses
{
    public class AnswerBoxResponseDto
    {
        public AnswerResponseDTO MainAnswer { get; set; }

        public List<AnswerResponseDTO> ChildAnswers { get; set; }

        public List<AnswerBoxResponseDto> ChildBoxAnswers { get; set; }
    }
}
