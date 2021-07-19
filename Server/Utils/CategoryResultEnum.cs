using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quanda.Shared.Enums
{
    /// <summary>
    ///     Enum opisujące możliwe komunikaty zwracane przez CategoriesRepository
    /// </summary>
    public enum CategoryResultEnum
    {
        CATEGORY_NOT_FOUND,
        CATEGORY_DELETED,
        CATEGORY_UPDATED,
        CATEGORY_CREATED,
        CATEGORY_DATABASE_ERROR
    }
}
