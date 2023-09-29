using Homepage.Models;
using Homepage.Shared.Interfaces;

namespace Homepage
{
    public class FakeApiService : ISecuredApiService
    {
        public FakeApiService(HttpClient httpClient) { }

        public Task<List<Guild>?> GetMutualGuildsAsync(string userId)
        {
            return Task.FromResult(new List<Guild>() { new Guild(1234567890, "Test Guild", "8117ddfab04262244f605cd338fdfb08", true, 2147483647, Array.Empty<string>()) })!;
        }

        public Task<SettingsDTO> GetSettingsAsync(string userId)
        {
            return Task.FromResult(new SettingsDTO() 
            {
                AdminRole = new DiscordEntity(123, "admin"),
                BrokerRole = new DiscordEntity(456, "broker"),
                MerchantRole = new DiscordEntity(789, "seller"),
                BuyerRole = new DiscordEntity(000, "buyer"),
                AuditLogChannel = new DiscordEntity(555, "Audit Log"),
                ResultLogChannel = new DiscordEntity(777, "Result Log"),
                Flags = (ulong)(Feature.ShillBidding | Feature.RecallListings | Feature.ConfirmTransactions | Feature.AcceptOffers),
                Offset = TimeSpan.FromHours(3),
                EconomyType = "Basic",
                DefaultBalance = 500,
                DefaultCurrency = "$",
                SnipeRange = TimeSpan.FromMinutes(1),
                SnipeExtension = TimeSpan.FromMinutes(5),
                MinimumDuration = TimeSpan.FromMinutes(5),
                MaximumDuration = TimeSpan.FromHours(5),
                DefaultDuration = TimeSpan.FromHours(1),
                BiddingRecallLimit = TimeSpan.FromMinutes(10),
                MaxListingsLimit = 5,
                AllowedListings = new List<string>() { "Standard Auction", "Standard Market", "Standard Trade", "Standard Giveaway"}
            });
        }
    }
}