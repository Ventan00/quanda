using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Quanda.Server.Extensions
{
    public static class HttpRequestUserIdExtension
    {
        public static int? IdUser;
        public static void SetUserId(this HttpRequest httpRequest, ClaimsPrincipal claimsPrincipal)
        {
            var nameIdentifier = claimsPrincipal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            IdUser = int.Parse(nameIdentifier);
        }

        public static int? GetUserId(this HttpRequest httpRequest)
        {
            return IdUser;
        }
    }
}
