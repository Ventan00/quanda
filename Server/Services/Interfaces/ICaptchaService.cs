using System.Threading.Tasks;

namespace Quanda.Server.Services.Interfaces
{
    public interface ICaptchaService
    {
        public Task<bool> VerifyCaptchaAsync(string responseToken);
    }
}
