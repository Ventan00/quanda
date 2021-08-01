using System;
using System.Collections.Generic;

namespace Quanda.Shared.Models
{
    public class Question
    {
        public Question()
        {
            QuestionTags = new HashSet<QuestionTag>();
            Answers = new HashSet<Answer>();
            Reports = new HashSet<Report>();
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

        public virtual User IdUserNavigation { get; set; }

        public virtual ICollection<QuestionTag> QuestionTags { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }
}
