using System;
using System.Collections.Generic;

namespace Quanda.Shared.Models
{
    public class Question
    {
        public Question()
        {
            QuestionCategories = new HashSet<QuestionCategory>();
            Notifications = new HashSet<Notification>();
            Answers = new HashSet<Answer>();
        }

        public int IdQuestion { get; set; }
        public string Header { get; set; }
        public string Description { get; set; }
        public DateTime PublishDate { get; set; }
        public int Views { get; set; }
        public bool IsFinished { get; set; }
        public bool IsModified { get; set; }
        public bool ToCheck { get; set; }
        public int IdUser { get; set; }

        public virtual ICollection<QuestionCategory> QuestionCategories { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual User IdUserNavigation { get; set; }
    }
}
