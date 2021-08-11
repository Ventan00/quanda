using System.Security.Claims;

namespace Quanda.Server.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static int? GetId(this ClaimsPrincipal claimsPrincipal)
        {
            var idUser = claimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            var canParse = int.TryParse(idUser, out var parsedIdUser);
            if (canParse)
                return parsedIdUser;
            return null;
        }
    }
}
