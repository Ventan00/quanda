namespace Quanda.Shared.Models
{
    public class Notification
    {
        public int IdUser { get; set; }
        public bool IsSeen { get; set; }
        public int Type { get; set; }
        public int? IdEntity { get; set; }
        public int Counter { get; set; }

        public virtual User IdUserNavigation { get; set; }
    }
}
