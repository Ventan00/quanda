namespace Quanda.Shared.Models
{
    public class QuestionTag
    {
        public int IdQuestion { get; set; }
        public int IdTag { get; set; }

        public virtual Tag IdTagNavigation { get; set; }
        public virtual Question IdQuestionNavigation { get; set; }
    }
}
