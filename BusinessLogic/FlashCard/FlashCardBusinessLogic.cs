using WebAPI.Services.FlashCard;

namespace WebAPI.BusinessLogic.FlashCard;

public class FlashCardBusinessLogic : IFlashCardBusinessLogic
{
    private readonly IFlashCardService _flashCardService;

    public FlashCardBusinessLogic(IFlashCardService flashCardService)
    {
        _flashCardService = flashCardService;
    }

    public async Task<Models.FlashCard?> GetFlashCardByIdAsync(int id)
    {
        return await _flashCardService.GetFlashCardByIdAsync(id);
    }

    public async Task<IEnumerable<Models.FlashCard>> GetFlashCardsByCollectionIdAsync(int collectionId)
    {
        return await _flashCardService.GetFlashCardsByCollectionIdAsync(collectionId);
    }

    public async Task<Models.FlashCard?> CreateFlashCardAsync(Models.FlashCard flashCard)
    {
        // Business logic: Validate flashcard data
        if (string.IsNullOrWhiteSpace(flashCard.Term) || string.IsNullOrWhiteSpace(flashCard.Definition))
        {
            return null;
        }

        // Additional business rules can be added here
        
        return await _flashCardService.CreateFlashCardAsync(flashCard);
    }

    public async Task<IEnumerable<Models.FlashCard>> CreateFlashCardsAsync(IEnumerable<Models.FlashCard> flashCards)
    {
        // Business logic: Validate all flashcards
        var validFlashCards = flashCards.Where(fc => 
            !string.IsNullOrWhiteSpace(fc.Term) && 
            !string.IsNullOrWhiteSpace(fc.Definition)).ToList();

        if (!validFlashCards.Any())
        {
            return Enumerable.Empty<Models.FlashCard>();
        }

        // Additional business rules can be added here
        
        return await _flashCardService.CreateFlashCardsAsync(validFlashCards);
    }

    public async Task<Models.FlashCard?> UpdateFlashCardAsync(int id, Models.FlashCard flashCard)
    {
        // Business logic: Validate update
        var existing = await _flashCardService.GetFlashCardByIdAsync(id);
        if (existing == null)
        {
            return null;
        }

        // Additional business rules can be added here
        
        existing.Term = flashCard.Term;
        existing.Definition = flashCard.Definition;
        existing.Score = flashCard.Score;

        return await _flashCardService.UpdateFlashCardAsync(existing);
    }

    public async Task<bool> DeleteFlashCardAsync(int id)
    {
        // Business logic: Check if flashcard can be deleted
        var flashCard = await _flashCardService.GetFlashCardByIdAsync(id);
        if (flashCard == null)
        {
            return false;
        }

        // Additional business rules can be added here
        
        return await _flashCardService.DeleteFlashCardAsync(id);
    }
}
