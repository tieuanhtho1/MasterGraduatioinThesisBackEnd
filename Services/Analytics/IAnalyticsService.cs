using WebAPI.Models.DTOs.Analytics;

namespace WebAPI.Services.Analytics;

public interface IAnalyticsService
{
    Task<AnalyticsResponse> GetUserAnalyticsAsync(int userId);
    Task<CollectionAnalyticsResponse?> GetCollectionAnalyticsAsync(int collectionId);
    Task<OverviewStats> GetOverviewStatsAsync(int userId);
    Task<LearningProgress> GetLearningProgressAsync(int userId);
    Task<List<CollectionStats>> GetTopCollectionsAsync(int userId, int limit = 5);
    Task<AverageScoreDistribution> GetAverageScoreDistributionAsync(int userId);
}
