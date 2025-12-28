namespace WebAPI.Models.DTOs.Analytics;

public class CollectionAnalyticsResponse
{
    public int CollectionId { get; set; }
    public string CollectionTitle { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int TotalFlashCards { get; set; }
    public int FlashCardsLearned { get; set; }
    public double AverageScore { get; set; } // Score/TimesLearned for learned cards
    public double CompletionRate { get; set; }
    public AverageScoreDistribution AverageScoreDistribution { get; set; } = new();
    public List<FlashCardPerformance> TopPerformingCards { get; set; } = new();
    public List<FlashCardPerformance> CardsNeedingReview { get; set; } = new();
}

public class FlashCardPerformance
{
    public int Id { get; set; }
    public string Term { get; set; } = string.Empty;
    public int Score { get; set; }
    public int TimesLearned { get; set; }
    public double AverageScore { get; set; } // Score/TimesLearned
}
