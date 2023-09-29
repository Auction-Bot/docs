using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace Homepage
{
    internal static class NavigationManagerExtensions
    {
        public static T? ExtractQueryStringByKey<T>(this NavigationManager navManager, string key)
        {
            var uri = navManager.ToAbsoluteUri(navManager.Uri);
            var queryDict = QueryHelpers.ParseQuery(uri.Query);

            if (queryDict.TryGetValue(key, out var queryValue))
            {
                return typeof(T) switch
                {
                    Type t when t.Equals(typeof(int)) && int.TryParse(queryValue, out var intValue) => (T)(object)intValue,
                    Type t when t.Equals(typeof(string)) => (T)(object)queryValue.ToString(),
                    _ => default,
                };
            }

            return default;
        }

        public static T? ExtractPathTail<T>(this NavigationManager navManager)
        {
            var currentUri = navManager.Uri.Remove(0, navManager.BaseUri.Length - 1);
            var lastElement = currentUri.Split("/", StringSplitOptions.RemoveEmptyEntries).LastOrDefault();

            if (lastElement is null) return default;

            return typeof(T) switch
            {
                Type t when t.Equals(typeof(ulong)) && ulong.TryParse(lastElement, out var ulongValue) => (T)(object)ulongValue,
                Type t when t.Equals(typeof(int)) && int.TryParse(lastElement, out var intValue) => (T)(object)intValue,
                Type t when t.Equals(typeof(string)) => (T)(object)lastElement,
                _ => default,
            };
        }

        public static string GetSection(this NavigationManager navMan)
        {
            var currentUri = navMan.Uri.Remove(0, navMan.BaseUri.Length - 1);
            var firstElement = currentUri.Split("/", StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();

            return firstElement ?? string.Empty;
        }

        public static string GetCommandLink(this NavigationManager navMan)
        {
            var currentUri = navMan.Uri.Remove(0, navMan.BaseUri.Length - 1);
            var secondElement = currentUri.Split("/", StringSplitOptions.RemoveEmptyEntries).ElementAtOrDefault(1);

            return secondElement ?? string.Empty;
        }

        public static bool IsHomePage(this NavigationManager navMan)
        {
            return navMan.Uri == navMan.BaseUri;
        }

        public static bool IsDashboard(this NavigationManager navMan)
        {
            return navMan.Uri.Contains("dashboard");
        }

        public static bool IsCommands(this NavigationManager navMan)
        {
            return navMan.Uri.Contains("commands");
        }

        public static bool IsFAQ(this NavigationManager navMan)
        {
            return navMan.Uri.Contains("faqs");
        }
    }
}
