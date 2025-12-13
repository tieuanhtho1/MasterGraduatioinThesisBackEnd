using WebAPI.Services.FlashCard;
using WebAPI.Models.DTOs.FlashCard;
using WebAPI.Helpers;

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

    public async Task<(IEnumerable<Models.FlashCard> flashCards, int totalCount, int totalPages)> GetFlashCardsByCollectionIdAsync(int collectionId, int pageNumber, int pageSize, string? searchText = null)
    {
        // Business logic: Validate pagination parameters
        (pageNumber, pageSize) = PaginationHelper.ValidatePaginationParameters(pageNumber, pageSize);

        var (flashCards, totalCount) = await _flashCardService.GetFlashCardsByCollectionIdAsync(collectionId, pageNumber, pageSize, searchText);
        var totalPages = pageSize == -1 ? 1 : PaginationHelper.CalculateTotalPages(totalCount, pageSize);

        return (flashCards, totalCount, totalPages);
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

    public async Task<(IEnumerable<Models.FlashCard> results, int createdCount, int updatedCount)> BulkCreateOrUpdateFlashCardsAsync(int collectionId, IEnumerable<FlashCardItem> flashCardItems)
    {
        var results = new List<Models.FlashCard>();
        var createdCount = 0;
        var updatedCount = 0;

        foreach (var fc in flashCardItems)
        {
            // Validate flashcard data
            if (string.IsNullOrWhiteSpace(fc.Term) || string.IsNullOrWhiteSpace(fc.Definition))
            {
                continue;
            }

            if (fc.Id.HasValue && fc.Id.Value > 0)
            {
                // Update existing flashcard
                var flashCard = new Models.FlashCard
                {
                    Term = fc.Term,
                    Definition = fc.Definition,
                    Score = fc.Score
                };

                var updated = await UpdateFlashCardAsync(fc.Id.Value, flashCard);
                if (updated != null)
                {
                    results.Add(updated);
                    updatedCount++;
                }
            }
            else
            {
                // Create new flashcard
                var flashCard = new Models.FlashCard
                {
                    Term = fc.Term,
                    Definition = fc.Definition,
                    Score = fc.Score,
                    FlashCardCollectionId = collectionId
                };

                var created = await CreateFlashCardAsync(flashCard);
                if (created != null)
                {
                    results.Add(created);
                    createdCount++;
                }
            }
        }

        return (results, createdCount, updatedCount);
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

    public async Task<(int deletedCount, int failedCount)> BulkDeleteFlashCardsAsync(IEnumerable<int> flashCardIds)
    {
        var deletedCount = 0;
        var failedCount = 0;

        foreach (var id in flashCardIds)
        {
            var result = await DeleteFlashCardAsync(id);
            if (result)
            {
                deletedCount++;
            }
            else
            {
                failedCount++;
            }
        }

        return (deletedCount, failedCount);
    }
}
