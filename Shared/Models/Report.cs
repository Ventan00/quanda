namespace Quanda.Shared.Models
{
    public class Report
    {
        public int IdReport { get; set; }
        public int IdEntity { get; set; }
        public int IdIssuer { get; set; }
        public int? IdMessage { get; set; }
        public int? IdAnswer { get; set; }
        public int? IdQuestion { get; set; }

        public virtual Message IdMessageNavigation { get; set; }
        public virtual User IdIssuerNavigation { get; set; }
        public virtual Answer IdAnswerNavigation { get; set; }
        public virtual Question IdQuestionNavigation { get; set; }
    }
}
