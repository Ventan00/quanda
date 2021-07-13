using System.Web;
using Microsoft.AspNetCore.Components;

namespace Quanda.Client.Extensions
{
    public static class NavigationManagerExtension
    {
        public static T? ExtractQueryStringByKey<T>(this NavigationManager navManager, string key)
        {
            var uri = navManager.ToAbsoluteUri(navManager.Uri).Query;

            var value = HttpUtility.ParseQueryString(uri).Get(key);
            if (value == null)
                return default;

            if (typeof(T) == typeof(int?))
            {
                var canParse = int.TryParse(value, out var result);
                if (canParse)
                    return (T)(object)result;
                return default;
            }

            if (typeof(T) == typeof(string))
                return (T)(object)value;

            return default;
        }
    }
}
