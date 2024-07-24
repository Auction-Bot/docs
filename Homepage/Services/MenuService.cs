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
        private readonly List<BotCommand> _slashCommands = new ApplicationCommandBuilder()
            .AddNavGroup("Server", false, new ApplicationCommandBuilder()
                .AddItem("Server Setup")
                .AddItem("Server Settings")
                .AddItem("Server Rooms")
                .AddItem("Server Room Add")
                .AddItem("Server Room Remove")
                .AddItem("Server Roles Set")
                .AddItem("Server Roles Clear")
                .AddItem("Server Announcements")
                .AddItem("Server Categories")
                .AddItem("Server Currencies")
                .AddItem("Server Fees")
                .AddItem("Server Listing Requirements")
                .AddItem("Server Reset"))
            .AddNavGroup("Auction", false, new ApplicationCommandBuilder()
                .AddItem("Auction Add Standard")
                .AddItem("Auction Add Sealed")
                .AddItem("Auction Add Live"))
            .AddNavGroup("Market", false, new ApplicationCommandBuilder()
                .AddItem("Market Add Standard")
                .AddItem("Market Add Flash")
                .AddItem("Market Add Multi-Item")
                .AddItem("Market Add Bulk"))
            .AddNavGroup("Trade", false, new ApplicationCommandBuilder()
                .AddItem("Trade Add Standard")
                .AddItem("Trade Add Request"))
            .AddNavGroup("Giveaway", false, new ApplicationCommandBuilder()
                .AddItem("Giveaway Add Standard")
                .AddItem("Giveaway Add Raffle"))
            .AddItem("Bid")
            .AddItem("Tips")
            .AddItem("Watchlist")
            .AddItem("Outbid Alerts")
            .AddItem("Auto-Reschedule")
            .AddNavGroup("Template", false, new ApplicationCommandBuilder()
                .AddItem("Template Create Auction")
                .AddItem("Template List Auction")
                .AddItem("Template Add Auction"))
            .AddNavGroup("Image", false, new ApplicationCommandBuilder()
                .AddItem("Image Upload")
                .AddItem("Image Link"))
            .AddNavGroup("Economy", false, new ApplicationCommandBuilder()
                .AddItem("Leaderboard")
                .AddItem("Balance")
                .AddItem("Give")
                .AddItem("Add-Money")
                .AddItem("Remove-Money")
                .AddItem("Reset-Balance")
                .AddItem("Reset-Economy"))
            .Build();

        private readonly List<BotCommand> _messageCommands = new ApplicationCommandBuilder()
            .AddItem("Review Transaction")
            .AddItem("Remove Review")
            .AddItem("Cancel Reschedule")
            .AddItem("Reroll Giveaway")
            .AddItem("View Logs")
            .Build();

        private readonly List<BotCommand> _userCommands = new ApplicationCommandBuilder()
            .AddItem("Merchant Rating")
            .Build();

        private readonly List<BotCommand> _hiddenCommands = new ApplicationCommandBuilder()
            .AddItem("Schedule")
            .Build();

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
