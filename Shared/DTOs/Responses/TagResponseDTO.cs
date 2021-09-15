namespace Quanda.Shared.DTOs.Responses
{
    public class TagResponseDTO
    {
        public int IdTag { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? IdMainTag { get; set; }
        public int? AmountOfQuestions { get; set; }
    }
}
