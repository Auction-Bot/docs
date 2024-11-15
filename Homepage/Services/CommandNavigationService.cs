using Microsoft.AspNetCore.Components;

namespace Homepage.Services
{
    public enum NavigationOrder { Previous, Next }
    public enum NavigationSection { Unspecified, SlashCommands, MessageCommands, UserCommands, HiddenCommands }

    public record NavigationFooterLink(string Name, string Link);

    public interface ICommandNavigationService
    {
        NavigationFooterLink? Next { get; }
        NavigationFooterLink? Previous { get; }
        NavigationSection Section { get; }
    }

    public class CommandNavigationService : ICommandNavigationService
    {
        private readonly NavigationManager _navigationManager;

        private string CurrentLink => _navigationManager.GetCommandLink();

        public NavigationFooterLink? Previous => GetNavigationLink(NavigationOrder.Previous);
        public NavigationFooterLink? Next => GetNavigationLink(NavigationOrder.Next);

        public NavigationSection Section
        {
            get
            {
                return _navigationManager.GetSection() switch
                {
                    "slashcommands" => NavigationSection.SlashCommands,
                    "messagecommands" => NavigationSection.MessageCommands,
                    "usercommands" => NavigationSection.UserCommands,
                    "hiddencommands" => NavigationSection.HiddenCommands,
                    _ => NavigationSection.Unspecified,
                };
            }
        }

        public CommandNavigationService(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        private NavigationFooterLink? GetNavigationLink(NavigationOrder order)
        {
            List<NavigationFooterLink> orderedLinks = GetOrderedMenuLinks(GetCurrentSection());

            var index = orderedLinks.FindIndex(e => e.Link == CurrentLink);
            var increment = order == NavigationOrder.Next ? 1 : -1;

            var position = index + increment;
            if (position < 0 || position >= orderedLinks.Count) return null;

            return orderedLinks.ElementAt(position);
        }

        private static List<NavigationFooterLink> GetOrderedMenuLinks(NavigationSection section)
        {
            var links = new List<NavigationFooterLink>();
            return links;
        }

        private NavigationSection GetCurrentSection()
        {
            return Section switch
            {
                NavigationSection.SlashCommands => NavigationSection.SlashCommands,
                NavigationSection.MessageCommands => NavigationSection.MessageCommands,
                NavigationSection.UserCommands => NavigationSection.UserCommands,
                NavigationSection.HiddenCommands => NavigationSection.HiddenCommands,
                _ => NavigationSection.Unspecified
            };
        }
    }
}
