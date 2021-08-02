using System;

namespace Quanda.Shared.Models
{
    public class TempUser
    {
        public int IdUser { get; set; }
        public string Code { get; set; }
        public DateTime ExpirationDate { get; set; }

        public virtual User IdUserNavigation { get; set; }
    }
}
