using System.Collections.Generic;

namespace Quanda.Shared.Models
{
    public class Answer
    {
        public Answer()
        {
            InverseIdRootAnswersNavigation = new HashSet<Answer>();
            RatingAnswers = new HashSet<RatingAnswer>();
        }

        public int IdAnswer { get; set; }
        public string Text { get; set; }
        public bool IsModified { get; set; }
        public int IdQuestion { get; set; }
        public int IdUser { get; set; }
        public int? IdRootAnswer { get; set; }

        public virtual User IdUserNavigation { get; set; }
        public virtual Question IdQuestionNavigation { get; set; }
        public virtual Answer IdRootAnswerNavigation { get; set; }
        public virtual ICollection<Answer> InverseIdRootAnswersNavigation { get; set; }
        public virtual ICollection<RatingAnswer> RatingAnswers { get; set; }

    }
}
