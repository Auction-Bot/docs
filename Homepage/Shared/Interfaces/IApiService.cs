using Homepage.Models;

namespace Homepage.Shared.Interfaces
{
    public interface IApiService
    {
        Task<StatisticsDTO?> GetBotStatistics();
    }
}