using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Models.DTOs.Analytics;

namespace WebAPI.Services.Analytics;

public class AnalyticsService : IAnalyticsService
{
    private readonly ApplicationDbContext _context;

    public AnalyticsService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AnalyticsResponse> GetUserAnalyticsAsync(int userId)
    {
        var analytics = new AnalyticsResponse
        {
            Overview = await GetOverviewStatsAsync(userId),
            LearningProgress = await GetLearningProgressAsync(userId),
            TopCollections = await GetTopCollectionsAsync(userId, 5),
            AverageScoreDistribution = await GetAverageScoreDistributionAsync(userId)
        };

        return analytics;
    }

    public async Task<CollectionAnalyticsResponse?> GetCollectionAnalyticsAsync(int collectionId)
    {
        var collection = await _context.FlashCardCollections
            .Include(c => c.FlashCards)
            .FirstOrDefaultAsync(c => c.Id == collectionId);

        if (collection == null)
            return null;

        var flashCards = collection.FlashCards;
        var totalCards = flashCards.Count;
        var cardsLearned = flashCards.Count(f => f.TimesLearned > 0);
        
        // Calculate average score using Score/TimesLearned for learned cards
        var learnedCards = flashCards.Where(f => f.TimesLearned > 0).ToList();
        var avgScore = learnedCards.Any() 
            ? learnedCards.Average(f => (double)f.Score / f.TimesLearned) 
            : 0;

        var analytics = new CollectionAnalyticsResponse
        {
            CollectionId = collection.Id,
            CollectionTitle = collection.Title,
            Description = collection.Description,
            TotalFlashCards = totalCards,
            FlashCardsLearned = cardsLearned,
            AverageScore = Math.Round(avgScore, 2),
            CompletionRate = totalCards > 0 ? Math.Round((double)cardsLearned / totalCards * 100, 2) : 0,
            AverageScoreDistribution = GetAverageScoreDistributionForCards(flashCards),
            TopPerformingCards = flashCards
                .OrderByDescending(f => f.Score)
                .ThenByDescending(f => f.TimesLearned)
                .Take(5)
                .Select(f => new FlashCardPerformance
                {
                    Id = f.Id,
                    Term = f.Term,
                    Score = f.Score,
                    TimesLearned = f.TimesLearned,
                    AverageScore = f.TimesLearned > 0 ? Math.Round((double)f.Score / f.TimesLearned, 2) : 0
                })
                .ToList(),
            CardsNeedingReview = flashCards
                .Where(f => f.Score < 60 || f.TimesLearned == 0)
                .OrderBy(f => f.Score)
                .Take(5)
                .Select(f => new FlashCardPerformance
                {
                    Id = f.Id,
                    Term = f.Term,
                    Score = f.Score,
                    TimesLearned = f.TimesLearned,
                    AverageScore = f.TimesLearned > 0 ? Math.Round((double)f.Score / f.TimesLearned, 2) : 0
                })
                .ToList()
        };

        return analytics;
    }

    public async Task<OverviewStats> GetOverviewStatsAsync(int userId)
    {
        var collections = await _context.FlashCardCollections
            .Where(c => c.UserId == userId)
            .Include(c => c.FlashCards)
            .ToListAsync();

        var allFlashCards = collections.SelectMany(c => c.FlashCards).ToList();
        var totalCards = allFlashCards.Count;
        var cardsLearned = allFlashCards.Count(f => f.TimesLearned > 0);
        var avgScore = totalCards > 0 ? allFlashCards.Average(f => f.Score) : 0;

        return new OverviewStats
        {
            TotalCollections = collections.Count,
            TotalFlashCards = totalCards,
            TotalFlashCardsLearned = cardsLearned,
            AverageScore = Math.Round(avgScore, 2)
        };
    }

    public async Task<LearningProgress> GetLearningProgressAsync(int userId)
    {
        var flashCards = await _context.FlashCardCollections
            .Where(c => c.UserId == userId)
            .SelectMany(c => c.FlashCards)
            .ToListAsync();

        var totalCards = flashCards.Count;
        var cardsToReview = flashCards.Count(f => f.TimesLearned == 0);
        var cardsMastered = flashCards.Count(f => f.TimesLearned > 0 && (double)f.Score / f.TimesLearned >= 3);
        var cardsInProgress = flashCards.Count(f => f.TimesLearned > 0 && (double)f.Score / f.TimesLearned >= 0 && (double)f.Score / f.TimesLearned < 3);
        var cardsNeedWork = flashCards.Count(f => f.TimesLearned > 0 && (double)f.Score / f.TimesLearned < 0);
        var cardsLearned = flashCards.Count(f => f.TimesLearned > 0);

        return new LearningProgress
        {
            CardsToReview = cardsToReview,
            CardsMastered = cardsMastered,
            CardsInProgress = cardsInProgress,
            CardsNeedWork = cardsNeedWork,
            CompletionRate = totalCards > 0 ? Math.Round((double)cardsLearned / totalCards * 100, 2) : 0
        };
    }

    public async Task<List<CollectionStats>> GetTopCollectionsAsync(int userId, int limit = 5)
    {
        var collections = await _context.FlashCardCollections
            .Where(c => c.UserId == userId)
            .Include(c => c.FlashCards)
            .ToListAsync();

        var stats = collections.Select(c =>
        {
            var totalCards = c.FlashCards.Count;
            var cardsLearned = c.FlashCards.Count(f => f.TimesLearned > 0);
            var avgScore = totalCards > 0 ? c.FlashCards.Average(f => f.Score) : 0;

            return new CollectionStats
            {
                CollectionId = c.Id,
                CollectionTitle = c.Title,
                FlashCardCount = totalCards,
                CardsLearned = cardsLearned,
                TotalTimesLearned = c.FlashCards.Sum(f => f.TimesLearned),
                AverageScore = Math.Round(avgScore, 2),
                CompletionRate = totalCards > 0 ? Math.Round((double)cardsLearned / totalCards * 100, 2) : 0
            };
        })
        .OrderByDescending(s => s.AverageScore)
        .ThenByDescending(s => s.CompletionRate)
        .Take(limit)
        .ToList();

        return stats;
    }

    public async Task<AverageScoreDistribution> GetAverageScoreDistributionAsync(int userId)
    {
        var flashCards = await _context.FlashCardCollections
            .Where(c => c.UserId == userId)
            .SelectMany(c => c.FlashCards)
            .ToListAsync();

        return GetAverageScoreDistributionForCards(flashCards);
    }

    private AverageScoreDistribution GetAverageScoreDistributionForCards(List<Models.FlashCard> flashCards)
    {
        // Only consider cards that have been learned at least once
        var learnedCards = flashCards.Where(f => f.TimesLearned > 0).ToList();
        
        return new AverageScoreDistribution
        {
            ScoreMinus5ToMinus3 = learnedCards.Count(f => {
                var avgScore = (double)f.Score / f.TimesLearned;
                return avgScore >= -5 && avgScore < -3;
            }),
            ScoreMinus3ToMinus1 = learnedCards.Count(f => {
                var avgScore = (double)f.Score / f.TimesLearned;
                return avgScore >= -3 && avgScore < -1;
            }),
            ScoreMinus1To1 = learnedCards.Count(f => {
                var avgScore = (double)f.Score / f.TimesLearned;
                return avgScore >= -1 && avgScore < 1;
            }),
            Score1To3 = learnedCards.Count(f => {
                var avgScore = (double)f.Score / f.TimesLearned;
                return avgScore >= 1 && avgScore < 3;
            }),
            Score3To5 = learnedCards.Count(f => {
                var avgScore = (double)f.Score / f.TimesLearned;
                return avgScore >= 3 && avgScore <= 5;
            })
        };
    }
}
