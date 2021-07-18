using System.Threading.Tasks;

namespace Quanda.Server.Services.Interfaces
{
    public interface ICaptchaService
    {
        /// <summary>
        /// Metoda weryfikująca rezultat captchy za pomocą api google
        /// </summary>
        /// <param name="responseToken">Token odpowiedzi captchy przesłany przez klienta</param>
        /// <returns></returns>
        public Task<bool> VerifyCaptchaAsync(string responseToken);
    }
}
