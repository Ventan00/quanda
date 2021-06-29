namespace Quanda.Shared.Models
{
    public class QuestionCategory
    {
        public int IdQuestion { get; set; }
        public int IdCategory { get; set; }

        public virtual Category IdCategoryNavigation { get; set; }
        public virtual Question IdQuestionNavigation { get; set; }
    }
}
