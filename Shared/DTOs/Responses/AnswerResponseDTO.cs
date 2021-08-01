using System.Collections.Generic;

namespace Quanda.Shared.DTOs.Responses
{
    public class AnswerResponseDTO
    {
        public int IdAnswer { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public bool IsModified { get; set; }
        public int? IdRootAnswer { get; set; }
        public int Mark { get; set; }
        public UserResponseDTO UserResponseDTO { get; set; }
        public List<AnswerResponseDTO> ChildAnswers { get; set; }
        public int AmountOfChildAnswers { get; set; }
    }
}
