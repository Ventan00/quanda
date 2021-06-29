namespace Quanda.Shared.Models
{
    public class Notification
    {
        public int IdUser { get; set; }
        public int IdQuestion { get; set; }

        public virtual User IdUserNavigation { get; set; }
        public virtual Question IdQuestionNavigation { get; set; }
    }
}
