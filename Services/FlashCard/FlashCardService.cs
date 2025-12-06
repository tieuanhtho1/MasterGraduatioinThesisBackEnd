using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Helpers;

namespace WebAPI.Services.FlashCard;

public class FlashCardService : IFlashCardService
{
    private readonly ApplicationDbContext _context;

    public FlashCardService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Models.FlashCard?> GetFlashCardByIdAsync(int id)
    {
        return await _context.FlashCards.FindAsync(id);
    }

    public async Task<IEnumerable<Models.FlashCard>> GetFlashCardsByCollectionIdAsync(int collectionId)
    {
        return await _context.FlashCards
            .Where(fc => fc.FlashCardCollectionId == collectionId)
            .ToListAsync();
    }

    public async Task<(IEnumerable<Models.FlashCard> flashCards, int totalCount)> GetFlashCardsByCollectionIdAsync(int collectionId, int pageNumber, int pageSize, string? searchText = null)
    {
        var query = _context.FlashCards
            .Where(fc => fc.FlashCardCollectionId == collectionId);

        // Apply search filter if searchText is provided
        if (!string.IsNullOrWhiteSpace(searchText))
        {
            query = query.Where(fc => 
                fc.Term.Contains(searchText) || 
                fc.Definition.Contains(searchText));
        }

        var totalCount = await query.CountAsync();

        var flashCards = await query
            .Skip(PaginationHelper.CalculateSkip(pageNumber, pageSize))
            .Take(pageSize)
            .ToListAsync();

        return (flashCards, totalCount);
    }

    public async Task<IEnumerable<Models.FlashCard>> GetAllFlashCardsAsync()
    {
        return await _context.FlashCards.ToListAsync();
    }

    public async Task<Models.FlashCard> CreateFlashCardAsync(Models.FlashCard flashCard)
    {
        _context.FlashCards.Add(flashCard);
        await _context.SaveChangesAsync();
        return flashCard;
    }

    public async Task<IEnumerable<Models.FlashCard>> CreateFlashCardsAsync(IEnumerable<Models.FlashCard> flashCards)
    {
        await _context.FlashCards.AddRangeAsync(flashCards);
        await _context.SaveChangesAsync();
        return flashCards;
    }

    public async Task<Models.FlashCard> UpdateFlashCardAsync(Models.FlashCard flashCard)
    {
        _context.FlashCards.Update(flashCard);
        await _context.SaveChangesAsync();
        return flashCard;
    }

    public async Task<bool> DeleteFlashCardAsync(int id)
    {
        var flashCard = await _context.FlashCards.FindAsync(id);
        if (flashCard == null)
        {
            return false;
        }

        _context.FlashCards.Remove(flashCard);
        await _context.SaveChangesAsync();
        return true;
    }
}
