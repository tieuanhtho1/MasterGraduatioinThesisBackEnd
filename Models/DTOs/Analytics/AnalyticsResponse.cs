namespace WebAPI.Models.DTOs.Analytics;

public class AnalyticsResponse
{
    public OverviewStats Overview { get; set; } = new();
    public LearningProgress LearningProgress { get; set; } = new();
    public List<CollectionStats> TopCollections { get; set; } = new();
    public ScoreDistribution ScoreDistribution { get; set; } = new();
}

public class OverviewStats
{
    public int TotalCollections { get; set; }
    public int TotalFlashCards { get; set; }
    public int TotalFlashCardsLearned { get; set; }
    public double AverageScore { get; set; }
}

public class LearningProgress
{
    public int CardsToReview { get; set; }
    public int CardsMastered { get; set; } // Score >= 80
    public int CardsInProgress { get; set; } // Score 40-79
    public int CardsNeedWork { get; set; } // Score < 40
    public double CompletionRate { get; set; } // Percentage of cards learned at least once
}

public class CollectionStats
{
    public int CollectionId { get; set; }
    public string CollectionTitle { get; set; } = string.Empty;
    public int FlashCardCount { get; set; }
    public int CardsLearned { get; set; }
    public int TotalTimesLearned { get; set; }
    public double AverageScore { get; set; }
    public double CompletionRate { get; set; }
}

public class ScoreDistribution
{
    public int Score0To20 { get; set; }
    public int Score21To40 { get; set; }
    public int Score41To60 { get; set; }
    public int Score61To80 { get; set; }
    public int Score81To100 { get; set; }
}
