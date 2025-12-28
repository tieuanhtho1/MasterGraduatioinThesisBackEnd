namespace WebAPI.Models.DTOs.Analytics;

public class AnalyticsResponse
{
    public OverviewStats Overview { get; set; } = new();
    public LearningProgress LearningProgress { get; set; } = new();
    public List<CollectionStats> TopCollections { get; set; } = new();
    public AverageScoreDistribution AverageScoreDistribution { get; set; } = new();
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

public class AverageScoreDistribution
{
    public int ScoreMinus5ToMinus3 { get; set; }
    public int ScoreMinus3ToMinus1 { get; set; }
    public int ScoreMinus1To1 { get; set; }
    public int Score1To3 { get; set; }
    public int Score3To5 { get; set; }
}
