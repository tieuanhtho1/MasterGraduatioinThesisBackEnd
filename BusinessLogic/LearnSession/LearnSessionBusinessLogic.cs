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

    public async Task<bool> UpdateFlashCardScoresAsync(Dictionary<int, int> scoreModifications)
    {
        if (scoreModifications == null || !scoreModifications.Any())
        {
            return false;
        }

        return await _learnSessionService.UpdateFlashCardScoresAsync(scoreModifications);
    }

    private List<Models.FlashCard> SelectWeightedRandomFlashCards(List<Models.FlashCard> flashCards, int count)
    {
        if (flashCards.Count <= count)
        {
            return flashCards;
        }

        // Calculate weights based on score (lower score = higher weight)
        // Using inverse scoring: weight = maxScore - score + 1
        var maxScore = flashCards.Max(fc => fc.Score);
        var weightedCards = flashCards.Select(fc => new
        {
            FlashCard = fc,
            Weight = maxScore - fc.Score + 1
        }).ToList();

        var totalWeight = weightedCards.Sum(wc => wc.Weight);
        var random = new Random();
        var selectedCards = new List<Models.FlashCard>();
        var remainingCards = new List<Models.FlashCard>(flashCards);

        for (int i = 0; i < count && remainingCards.Any(); i++)
        {
            var randomValue = random.Next(0, totalWeight);
            var cumulativeWeight = 0;
            Models.FlashCard? selectedCard = null;

            foreach (var card in remainingCards)
            {
                var weight = maxScore - card.Score + 1;
                cumulativeWeight += weight;

                if (randomValue < cumulativeWeight)
                {
                    selectedCard = card;
                    break;
                }
            }

            if (selectedCard != null)
            {
                selectedCards.Add(selectedCard);
                remainingCards.Remove(selectedCard);
                totalWeight -= (maxScore - selectedCard.Score + 1);
            }
        }

        return selectedCards;
    }
}
