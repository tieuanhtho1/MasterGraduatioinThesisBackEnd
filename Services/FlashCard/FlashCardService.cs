using Microsoft.EntityFrameworkCore;
using WebAPI.Data;

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
