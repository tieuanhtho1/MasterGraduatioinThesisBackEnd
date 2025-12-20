using WebAPI.Services.LearnSession;

namespace WebAPI.BusinessLogic.LearnSession;

public class LearnSessionBusinessLogic : ILearnSessionBusinessLogic
{
    private readonly ILearnSessionService _learnSessionService;

    public LearnSessionBusinessLogic(ILearnSessionService learnSessionService)
    {
        _learnSessionService = learnSessionService;
    }

    public async Task<List<Models.FlashCard>> GetLearnSessionFlashCardsAsync(int collectionId, int count = 10)
    {
        if (collectionId <= 0)
        {
            return new List<Models.FlashCard>();
        }

        if (count <= 0)
        {
            count = 10;
        }

        // Get all flashcards from collection tree
        var allFlashCards = await _learnSessionService.GetFlashCardsFromCollectionTreeAsync(collectionId);

        if (!allFlashCards.Any())
        {
            return new List<Models.FlashCard>();
        }

        // Apply weighted random selection logic
        return SelectWeightedRandomFlashCards(allFlashCards, count);
    }

    public async Task<bool> UpdateFlashCardScoresAsync(List<Models.DTOs.LearnSession.FlashCardScoreUpdate> scoreUpdates)
    {
        if (scoreUpdates == null || !scoreUpdates.Any())
        {
            return false;
        }

        return await _learnSessionService.UpdateFlashCardScoresAsync(scoreUpdates);
    }

    private static readonly Random _random = new Random();


    private List<Models.FlashCard> SelectWeightedRandomFlashCards(
        List<Models.FlashCard> flashCards,
        int count)
    {
        // =========================
        // 🔧 TUNING PARAMETERS
        // =========================

        const double NEW_CARD_EFFECTIVE_SCORE = -6.0;
        const double WEIGHT_STRENGTH_MULTIPLIER = 1.0; // >1 = harsher, <1 = gentler

        // =========================
        // 🛡️ GUARDS
        // =========================

        if (flashCards == null || flashCards.Count == 0)
            return new List<Models.FlashCard>();

        if (flashCards.Count <= count)
            return flashCards;

        // =========================
        // 📊 STEP 1: Effective Score
        // =========================

        var cards = flashCards.Select(fc =>
        {
            double effectiveScore;

            if (fc.TimesLearned == 0)
            {
                // Never-learned cards
                effectiveScore = NEW_CARD_EFFECTIVE_SCORE;
            }
            else
            {
                effectiveScore = (double)fc.Score / fc.TimesLearned;
            }

            return new
            {
                FlashCard = fc,
                EffectiveScore = effectiveScore
            };
        }).ToList();

        // =========================
        // ⚖️ STEP 2: Score → Weight
        // =========================

        var weightedCards = cards.Select(c => new
        {
            c.FlashCard,
            Weight = Math.Exp(-c.EffectiveScore * WEIGHT_STRENGTH_MULTIPLIER)
        }).ToList();

        var selectedCards = new List<Models.FlashCard>();
        var remaining = weightedCards.ToList();

        // =========================
        // 🎲 STEP 3: Weighted Pick
        // =========================

        for (int i = 0; i < count && remaining.Any(); i++)
        {
            var totalWeight = remaining.Sum(c => c.Weight);
            var roll = _random.NextDouble() * totalWeight;

            double cumulative = 0;

            foreach (var card in remaining)
            {
                cumulative += card.Weight;
                if (roll <= cumulative)
                {
                    selectedCards.Add(card.FlashCard);
                    remaining.Remove(card);
                    break;
                }
            }
        }
        return selectedCards;
    }
}

