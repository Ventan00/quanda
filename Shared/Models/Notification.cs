namespace Quanda.Shared.Models
{
    public class Notification
    {
        public int IdUser { get; set; }
        public bool IsSeen { get; set; }
        /// <summary>
        /// Property dotyczące wartości enuma informującego o rodzaju powiadomienia(odpowiedź/pytanie/premium konto).
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// Wartość wynosi Id obiektu z bazy gdy typ notifkacji odnosi się do pytania lub odpowiedzi. W przypadku powiadomienia o koncie premium wynosi null.
        /// </summary>
        public int? IdEntity { get; set; }
        public int Counter { get; set; }

        public virtual User IdUserNavigation { get; set; }
    }
}
