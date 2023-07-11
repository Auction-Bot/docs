using Homepage.Models;

namespace Homepage.Services
{
    public interface IMenuService
    {
        public record DocsLink(string Href, string Title, string Group);

        IEnumerable<BotCommand> SlashCommands { get; }
        IEnumerable<BotCommand> MessageCommands { get; }
        IEnumerable<BotCommand> UserCommands { get; }
        IEnumerable<BotCommand> HiddenCommands { get; }

        BotCommand GetParent(Type type);
        BotCommand GetCommand(Type type);
    }

    public class MenuService : IMenuService
    {
        private readonly List<BotCommand> _slashCommands = new ApplicationCommand()
            .AddNavGroup("Auction",false, new ApplicationCommand()
                .AddItem("/Auction Add Standard")
                .AddItem("/Auction Add Vickrey")
                .AddItem("/Auction Add Live"))
            .AddNavGroup("Market", false, new ApplicationCommand()
                .AddItem("/Market Add Standard")
                .AddItem("/Market Add Flash")
                .AddItem("/Market Add Multi-Item")
                .AddItem("/Market Add Bulk"))
            .AddNavGroup("Trade", false, new ApplicationCommand()
                .AddItem("/Trade Add Standard")
                .AddItem("/Trade Add Request"))
            .AddNavGroup("Giveaway", false, new ApplicationCommand()
                .AddItem("/Giveaway Add Standard")
                .AddItem("/Giveaway Add Raffle"))
            .AddItem("/Bid")
            .AddItem("/Tips")
            .AddItem("/Watchlist")
            .AddItem("/Outbid Alerts")
            .AddItem("/Auto-Reschedule")
            .AddNavGroup("Image", false, new ApplicationCommand()
                .AddItem("/Image Upload")
                .AddItem("/Image Link"))
            .AddNavGroup("Economy", false, new ApplicationCommand()
                .AddItem("/Leaderboard")
                .AddItem("/Balance")
                .AddItem("/Give")
                .AddItem("/Add-Money")
                .AddItem("/Remove-Money")
                .AddItem("/Reset-Balance")
                .AddItem("/Reset-Economy"))
            .AddNavGroup("Server", false, new ApplicationCommand()
                .AddItem("/Server Setup")
                .AddItem("/Server Settings")
                .AddItem("/Server Rooms")
                .AddItem("/Server Room Add")
                .AddItem("/Server Room Remove")
                .AddItem("/Server Roles Set")
                .AddItem("/Server Roles Clear")
                .AddItem("/Server Categories")
                .AddItem("/Server Currencies")
                .AddItem("/Server Listing Requirements")
                .AddItem("/Server Reset"))
            .GetCommandsSortedByName();

        private readonly List<BotCommand> _messageCommands = new ApplicationCommand()
            .AddItem("Review Transaction")
            .AddItem("Remove Review")
            .AddItem("Cancel Reschedule")
            .GetCommandsSortedByName();

        private readonly List<BotCommand> _userCommands = new ApplicationCommand()
            .AddItem("Merchant Rating")
            .GetCommandsSortedByName();

        private readonly List<BotCommand> _hiddenCommands = new ApplicationCommand()
            .AddItem("Schedule")
            .GetCommandsSortedByName();

        public IEnumerable<BotCommand> SlashCommands => _slashCommands;
        public IEnumerable<BotCommand> MessageCommands => _messageCommands;
        public IEnumerable<BotCommand> UserCommands => _userCommands;
        public IEnumerable<BotCommand> HiddenCommands => _hiddenCommands;

        public BotCommand GetCommand(Type type)
        {
            throw new NotImplementedException();
        }

        public BotCommand GetParent(Type type)
        {
            throw new NotImplementedException();
        }
    }
}
