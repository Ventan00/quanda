namespace Quanda.Shared
{
    /// <summary>
    ///     Klasa przechowująca wartości współdzielone między Klientem a serwerem
    /// </summary>
    public class Config
    {
        /// <summary>
        ///     Wartość która opisuje ile pytań ma załadować klient na jednej stronie z pytaniami
        /// </summary>
        public static readonly int QUESTIONS_PAGINATION_TAKE_SKIP = 10;
    }
}