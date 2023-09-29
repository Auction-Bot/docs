namespace Homepage.Models
{
    public record DiscordEntity(ulong Id, string Name);
    public record StatisticsDTO(string Servers, string Showrooms, string Users, string Listings);
    public record Guild(ulong Id, string Name, string Icon, bool Owner, ulong Permissions, string[] Features);
}
