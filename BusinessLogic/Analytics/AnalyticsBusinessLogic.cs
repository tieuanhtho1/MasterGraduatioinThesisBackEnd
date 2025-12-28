using WebAPI.Models.DTOs.Analytics;
using WebAPI.Services.Analytics;
using WebAPI.Services.FlashCardCollection;

namespace WebAPI.BusinessLogic.Analytics;

public class AnalyticsBusinessLogic : IAnalyticsBusinessLogic
{
    private readonly IAnalyticsService _analyticsService;
    private readonly IFlashCardCollectionService _collectionService;

    public AnalyticsBusinessLogic(
        IAnalyticsService analyticsService,
        IFlashCardCollectionService collectionService)
    {
        _analyticsService = analyticsService;
        _collectionService = collectionService;
    }

    public async Task<AnalyticsResponse> GetUserAnalyticsAsync(int userId)
    {
        // Business logic: Validate user has access
        return await _analyticsService.GetUserAnalyticsAsync(userId);
    }

    public async Task<CollectionAnalyticsResponse?> GetCollectionAnalyticsAsync(int collectionId, int userId)
    {
        // Business logic: Verify collection belongs to user
        var collection = await _collectionService.GetCollectionByIdAsync(collectionId);
        
        if (collection == null || collection.UserId != userId)
        {
            return null;
        }

        return await _analyticsService.GetCollectionAnalyticsAsync(collectionId);
    }

    public async Task<OverviewStats> GetOverviewStatsAsync(int userId)
    {
        return await _analyticsService.GetOverviewStatsAsync(userId);
    }

    public async Task<LearningProgress> GetLearningProgressAsync(int userId)
    {
        return await _analyticsService.GetLearningProgressAsync(userId);
    }
}
