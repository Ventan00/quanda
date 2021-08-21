using System;
using System.Collections.Generic;

namespace Quanda.Shared.DTOs.Responses
{
    public class GetQuestionsDTO
    {
        public int QuestionsCount { get; set; }
        public List<QuestionGetQuestionsDTO> StandardQuestions { get; set; }
        public List<QuestionGetQuestionsDTO> ExtraQuestions { get; set; }
    }
}
