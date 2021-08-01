namespace Quanda.Shared.Models
{
    public class TagUser
    {
        public int IdUser { get; set; }
        public int IdTag { get; set; }
        public int Points { get; set; }

        public virtual Tag IdTagNavigation { get; set; }
        public virtual User IdUserNavigation { get; set; }
    }
}
