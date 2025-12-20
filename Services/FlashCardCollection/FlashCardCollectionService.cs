using Microsoft.EntityFrameworkCore;
using WebAPI.Data;

namespace WebAPI.Services.FlashCardCollection;

public class FlashCardCollectionService : IFlashCardCollectionService
{
    private readonly ApplicationDbContext _context;

    public FlashCardCollectionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Models.FlashCardCollection?> GetCollectionByIdAsync(int id)
    {
        return await _context.FlashCardCollections
            .Include(c => c.FlashCards)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Models.FlashCardCollection>> GetCollectionsByUserIdAsync(int userId)
    {
        return await _context.FlashCardCollections
            .Where(c => c.UserId == userId)
            .Include(c => c.FlashCards)
            .ToListAsync();
    }

    public async Task<IEnumerable<Models.FlashCardCollection>> GetChildrenByParentIdAsync(int parentId)
    {
        return await _context.FlashCardCollections
            .Where(c => c.ParentId == parentId)
            .Include(c => c.FlashCards)
            .Include(c => c.Children)
            .ToListAsync();
    }

    public async Task<IEnumerable<Models.FlashCardCollection>> GetAllCollectionsAsync()
    {
        return await _context.FlashCardCollections
            .Include(c => c.FlashCards)
            .ToListAsync();
    }

    public async Task<Models.FlashCardCollection> CreateCollectionAsync(Models.FlashCardCollection collection)
    {
        _context.FlashCardCollections.Add(collection);
        await _context.SaveChangesAsync();
        return collection;
    }

    public async Task<Models.FlashCardCollection> UpdateCollectionAsync(Models.FlashCardCollection collection)
    {
        _context.FlashCardCollections.Update(collection);
        await _context.SaveChangesAsync();
        return collection;
    }

    public async Task<bool> DeleteCollectionAsync(int id)
    {
        var collection = await _context.FlashCardCollections.FindAsync(id);
        var childCollections = await _context.FlashCardCollections
                        .Where(c => c.ParentId == id)
                        .ToListAsync();
        if (collection == null || childCollections == null)
        {
            return false;
        }

        if (childCollections.Count > 0)
        {
            foreach (var childCollection in childCollections)
            {
                childCollection.ParentId = collection.ParentId;
            }
        }

        _context.FlashCardCollections.Remove(collection);

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<int> GetTotalFlashCardCountAsync(int collectionId)
    {
        // Get all descendant collection IDs (including the root collection)
        var collectionIds = await GetAllDescendantCollectionIdsAsync(collectionId);

        // Count all flashcards from these collections in a single query
        var count = await _context.FlashCards
            .Where(fc => collectionIds.Contains(fc.FlashCardCollectionId))
            .CountAsync();

        return count;
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
