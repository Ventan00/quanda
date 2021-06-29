using System.Collections.Generic;

namespace Quanda.Shared.Models
{
    public class Service
    {
        public Service()
        {
            Users = new HashSet<User>();
        }

        public int IdService { get; set; }
        public string Name { get; set; }
        public string Connection { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
