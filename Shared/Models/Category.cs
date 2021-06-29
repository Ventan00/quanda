using System.Collections.Generic;

namespace Quanda.Shared.Models
{
    public class Category
    {
        public Category()
        {
            QuestionCategories = new HashSet<QuestionCategory>();
            InverseIdMainCategoriesNavigation = new HashSet<Category>();
        }

        public int IdCategory { get; set; }
        public string Name { get; set; }
        public int? IdMainCategory { get; set; }

        public virtual ICollection<QuestionCategory> QuestionCategories { get; set; }
        public virtual Category IdMainCategoryNavigation { get; set; }
        public virtual ICollection<Category> InverseIdMainCategoriesNavigation { get; set; }
    }
}
