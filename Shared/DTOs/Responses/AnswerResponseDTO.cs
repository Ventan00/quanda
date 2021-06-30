namespace Quanda.Shared.DTOs.Responses
{
    public class AnswerResponseDTO
    {
        public int IdAnswer { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public bool IsModified { get; set; }
        public int IdUser { get; set; }
        public int? IdRootAnswer { get; set; }
    }
}
