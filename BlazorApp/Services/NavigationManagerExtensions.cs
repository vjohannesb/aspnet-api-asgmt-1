using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace BlazorApp.Services
{
    // Huvudsakligen fr. https://chrissainty.com/working-with-query-strings-in-blazor/
    public static class NavigationManagerExtensions
    {
        public static bool TryGetQueryString<T>(this NavigationManager navigationManager, string key, out T value)
        {
            var url = navigationManager.ToAbsoluteUri(navigationManager.Uri);

            if (QueryHelpers.ParseQuery(url.Query).TryGetValue(key, out var queryValue))
            {
                if (typeof(T) == typeof(string))
                {
                    value = (T)(object)queryValue.ToString();
                    return true;
                }
            }
            value = default;
            return false;
        }
    }
}
