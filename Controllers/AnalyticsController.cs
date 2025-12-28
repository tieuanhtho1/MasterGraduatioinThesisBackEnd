using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.BusinessLogic.Analytics;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AnalyticsController : ControllerBase
{
    private readonly IAnalyticsBusinessLogic _analyticsBusinessLogic;

    public AnalyticsController(IAnalyticsBusinessLogic analyticsBusinessLogic)
    {
        _analyticsBusinessLogic = analyticsBusinessLogic;
    }

    /// <summary>
    /// Get comprehensive analytics for a specific user
    /// </summary>
    [HttpGet("{userId}")]
    public async Task<IActionResult> GetUserAnalytics(int userId)
    {
        var analytics = await _analyticsBusinessLogic.GetUserAnalyticsAsync(userId);
        return Ok(analytics);
    }

    /// <summary>
    /// Get analytics for a specific collection
    /// </summary>
    [HttpGet("{userId}/collection/{collectionId}")]
    public async Task<IActionResult> GetCollectionAnalytics(int userId, int collectionId)
    {
        var analytics = await _analyticsBusinessLogic.GetCollectionAnalyticsAsync(collectionId, userId);
        
        if (analytics == null)
        {
            return NotFound(new { message = "Collection not found or access denied" });
        }

        return Ok(analytics);
    }

    /// <summary>
    /// Get overview statistics for a specific user
    /// </summary>
    [HttpGet("{userId}/overview")]
    public async Task<IActionResult> GetOverviewStats(int userId)
    {
        var stats = await _analyticsBusinessLogic.GetOverviewStatsAsync(userId);
        return Ok(stats);
    }

    /// <summary>
    /// Get learning progress for a specific user
    /// </summary>
    [HttpGet("{userId}/progress")]
    public async Task<IActionResult> GetLearningProgress(int userId)
    {
        var progress = await _analyticsBusinessLogic.GetLearningProgressAsync(userId);
        return Ok(progress);
    }
}
