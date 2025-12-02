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
        if (collection == null)
        {
            return false;
        }

        _context.FlashCardCollections.Remove(collection);
        await _context.SaveChangesAsync();
        return true;
    }
}
