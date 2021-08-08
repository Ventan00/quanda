using System;
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
        public int IsLiked { get; set; }
        public DateTime PublishDate { get; set; }
        public int AmountOfAnswerChildren { get; set; }
        public UserResponseDTO User { get; set; }
        public List<AnswerResponseDTO> AnswerChildren { get; set; }

    }
}
