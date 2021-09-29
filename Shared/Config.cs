namespace Quanda.Shared
{
    /// <summary>
    ///     Klasa przechowująca wartości współdzielone między Klientem a serwerem
    /// </summary>
    public static class Config
    {
        /// <summary>
        ///     Wartość która opisuje ile pytań ma załadować klient na jednej stronie z pytaniami
        /// </summary>
        public static readonly int QUESTIONS_PAGINATION_TAKE_SKIP = 10;

        /// <summary>
        ///     Wartość która opisuje ile stron może maksymalnie być wyświetlana na komponencie pagination
        /// </summary>
        public static readonly int MAX_ITEMS_AVILABLE_ON_PAGINATION_PAGE = 6;

        /// <summary>
        ///     Wartość która opisuje ile odpowiedzi ma załadować klient na jednej stronie.
        /// </summary>
        public static readonly int ANSWERS_PAGE_SIZE = 10;

        /// <summary>
        ///     Wartość która opisuje ile pytan ma załadować klient na jednej stronie pytan wyswietlanych przy jego profilu.
        /// </summary>
        public static readonly int ProfileQuestionsPageSize = 5;
      
        /// <summary>
        ///     Wartość która opisuje ile tagów ma załadować klient na jednej stronie.
        /// </summary>
        public static readonly int TagsPageSize = 16;
    }
}