using System;

namespace Quanda.Shared.Models
{
    public class UserRole
    {
        public int IdRole { get; set; }
        public int IdUser { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public virtual Role IdRoleNavigation { get; set; }
        public virtual User IdUserNavigation { get; set; }
    }
}
