using System.Collections.Generic;

namespace Quanda.Shared.DTOs.Responses
{
    public class GetProfileQuestionsResponseDto
    {
        public IEnumerable<QuestionInProfileResponseDto> Questions { get; set; }
        public int AmountOfAllQuestions { get; set; }
    }
}
