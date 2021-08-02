using System.Collections.Generic;

namespace Quanda.Shared.Models
{
    public class Thread
    {
        public Thread()
        {
            Messages = new HashSet<Message>();
        }
        public int IdThread { get; set; }
        public int IdReceiver { get; set; }
        public int IdSender { get; set; }
        public string Header { get; set; }

        public virtual User IdReceiverNavigation { get; set; }
        public virtual User IdSenderNavigation { get; set; }

        public virtual ICollection<Message> Messages { get; set; }
    }
}
