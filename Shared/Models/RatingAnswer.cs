namespace Quanda.Shared.Models
{
    public class RatingAnswer
    {
        public int IdAnswer { get; set; }
        public int IdUser { get; set; }
        public bool Value { get; set; }

        public virtual Answer IdAnswerNavigation { get; set; }
        public virtual User IdUserNavigation { get; set; }
    }
}
