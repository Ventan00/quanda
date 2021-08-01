using System.Collections.Generic;

namespace Quanda.Shared.Models
{
    public class Tag
    {
        public Tag()
        {
            QuestionTags = new HashSet<QuestionTag>();
            InverseIdMainTagsNavigation = new HashSet<Tag>();
            TagUsers = new HashSet<TagUser>();
        }

        public int IdTag { get; set; }
        public string Name { get; set; }
        public int? IdMainTag { get; set; }
        public string Description { get; set; }

        public virtual ICollection<QuestionTag> QuestionTags { get; set; }
        public virtual Tag IdMainTagNavigation { get; set; }
        public virtual ICollection<Tag> InverseIdMainTagsNavigation { get; set; }
        public virtual ICollection<TagUser> TagUsers { get; set; }
    }
}
