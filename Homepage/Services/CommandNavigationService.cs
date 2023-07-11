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
        private readonly IMenuService _menuService;

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

        public CommandNavigationService(NavigationManager navigationManager, IMenuService menuService)
        {
            _navigationManager = navigationManager;
            _menuService = menuService;
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

        private List<NavigationFooterLink> GetOrderedMenuLinks(NavigationSection section)
        {
            //var menuElements = section == NavigationSection.Components ? _menuService.SlashCommands : _menuService.MessageCommands;
            var links = new List<NavigationFooterLink>();
            //foreach (var menuElement in menuElements)
            //{
            //    if (menuElement.Link != null)
            //    {
            //        var link = section == NavigationSection.Api
            //            ? ApiLink.GetApiLinkFor(menuElement.Type).Split("/").Last()
            //            : menuElement.Link;

            //        var name = menuElement.Name;

            //        links.Add(new NavigationFooterLink(name, link));
            //    }

            //    if (menuElement.GroupCommands != null)
            //    {
            //        links.AddRange(menuElement.GroupCommands.Select(i => new NavigationFooterLink(i.Name, i.Link))
            //            .OrderBy(i => i.Link));
            //    }
            //}

            ;
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
