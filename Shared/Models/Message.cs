using System;
using System.Collections.Generic;

namespace Quanda.Shared.Models
{
    public class Message
    {
        public Message()
        {
            Reports = new HashSet<Report>();
        }
        public int IdMessage { get; set; }
        public int IdSender { get; set; }
        public bool IsSeen { get; set; }
        public int IdThread { get; set; }
        public DateTime CreationTime { get; set; }
        public string Text { get; set; }

        public virtual User IdSenderNavigation { get; set; }
        public virtual Thread IdThreadNavigation { get; set; }

        public virtual ICollection<Report> Reports { get; set; }
    }
}
