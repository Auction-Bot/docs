namespace Homepage.Models
{
    public class SettingsDTO
    {
        public DiscordEntity? AdminRole { get; set; }
        public DiscordEntity? BuyerRole { get; set; }
        public DiscordEntity? BrokerRole { get; set; }
        public DiscordEntity? MerchantRole { get; set; }
        public DiscordEntity? AuditLogChannel { get; set; }
        public DiscordEntity? ResultLogChannel { get; set; }
        public ulong Flags { get; set; }
        public TimeSpan Offset { get; set; }
        public string EconomyType { get; set; } = string.Empty;
        public string DefaultCurrency { get; set; } = string.Empty;
        public decimal DefaultBalance { get; set; }
        public TimeSpan SnipeRange { get; set; }
        public TimeSpan SnipeExtension { get; set; }
        public TimeSpan MinimumDuration { get; set; }
        public TimeSpan MaximumDuration { get; set; }
        public TimeSpan DefaultDuration { get; set; }
        public TimeSpan BiddingRecallLimit { get; set; }
        public int MaxListingsLimit { get; set; }
        public List<string> AllowedListings { get; set; } = new List<string>();
    }

    [Flags]
    public enum Feature : ulong
    {
        None = 0,
        AcceptOffers = 1 << 0,
        ShillBidding = 1 << 1,
        RecallListings = 1 << 2,
        HideMinMaxButtons = 1 << 3,
        ConfirmTransactions = 1 << 6,
        AttachLogs = 1 << 7
    }
}
