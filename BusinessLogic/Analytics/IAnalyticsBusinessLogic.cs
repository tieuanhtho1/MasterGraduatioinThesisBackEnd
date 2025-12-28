using WebAPI.Models.DTOs.Analytics;

namespace WebAPI.BusinessLogic.Analytics;

public interface IAnalyticsBusinessLogic
{
    Task<AnalyticsResponse> GetUserAnalyticsAsync(int userId);
    Task<CollectionAnalyticsResponse?> GetCollectionAnalyticsAsync(int collectionId, int userId);
    Task<OverviewStats> GetOverviewStatsAsync(int userId);
    Task<LearningProgress> GetLearningProgressAsync(int userId);
}
