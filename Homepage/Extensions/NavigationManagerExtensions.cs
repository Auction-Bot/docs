using Microsoft.AspNetCore.Components;

namespace Homepage
{
    internal static class NavigationManagerExtensions
    {
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
    }
}
