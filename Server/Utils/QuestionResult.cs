using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quanda.Server.Utils
{
    public enum QuestionResult
    {
        QUESTION_NOT_FOUND,
        QUESTION_DELETED,
        QUESTION_ADDED,
        QUESTION_UPDATED,
        QUESTION_SET_TO_FINISHED,
        QUESTION_CHANGED_TOCHECK_STATUS,
        QUESTION_DATABASE_ERROR,
    }
}
