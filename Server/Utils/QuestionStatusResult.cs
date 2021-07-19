namespace Quanda.Server.Utils
{
    /// <summary>
    ///     Enum opisujące możliwe komunikaty zwracane przez QuestionRepository
    /// </summary>
    public enum QuestionStatusResult
    {
        QUESTION_NOT_FOUND,
        QUESTION_DELETED,
        QUESTION_ADDED,
        QUESTION_UPDATED,
        QUESTION_SET_TO_FINISHED,
        QUESTION_CHANGED_TOCHECK_STATUS,
        QUESTION_DATABASE_ERROR
    }
}