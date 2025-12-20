using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Models.DTOs.LearnSession;

namespace WebAPI.Services.LearnSession;

public class LearnSessionService : ILearnSessionService
{
    private readonly ApplicationDbContext _context;

    public LearnSessionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Models.FlashCard>> GetFlashCardsFromCollectionTreeAsync(int collectionId)
    {
        // Get all descendant collection IDs (including the root collection)
        var collectionIds = await GetAllDescendantCollectionIdsAsync(collectionId);

        // Get all flashcards from these collections
        var allFlashCards = await _context.FlashCards
            .Where(fc => collectionIds.Contains(fc.FlashCardCollectionId))
            .ToListAsync();

        return allFlashCards;
    }

    public async Task<bool> UpdateFlashCardScoresAsync(List<FlashCardScoreUpdate> scoreUpdates)
    {
        foreach (var update in scoreUpdates)
        {
            var flashCard = await _context.FlashCards.FindAsync(update.FlashCardId);
            if (flashCard != null)
            {
                flashCard.Score += update.ScoreModification;
                flashCard.TimesLearned += update.TimesLearned;
            }
        }

        await _context.SaveChangesAsync();
        return true;
    }

    private async Task<List<int>> GetAllDescendantCollectionIdsAsync(int collectionId)
    {
        var result = new List<int> { collectionId };
        var queue = new Queue<int>();
        queue.Enqueue(collectionId);

        while (queue.Count > 0)
        {
            var currentId = queue.Dequeue();
            var children = await _context.FlashCardCollections
                .Where(c => c.ParentId == currentId)
                .Select(c => c.Id)
                .ToListAsync();

            foreach (var childId in children)
            {
                result.Add(childId);
                queue.Enqueue(childId);
            }
        }

        return result;
    }
}
