using WebAPI.Models;
using WebAPI.Models.DTOs.FlashCard;

namespace WebAPI.BusinessLogic.FlashCard;

public interface IFlashCardBusinessLogic
{
    Task<Models.FlashCard?> GetFlashCardByIdAsync(int id);
    Task<IEnumerable<Models.FlashCard>> GetFlashCardsByCollectionIdAsync(int collectionId);
    Task<(IEnumerable<Models.FlashCard> flashCards, int totalCount, int totalPages)> GetFlashCardsByCollectionIdAsync(int collectionId, int pageNumber, int pageSize, string? searchText = null);
    Task<Models.FlashCard?> CreateFlashCardAsync(Models.FlashCard flashCard);
    Task<IEnumerable<Models.FlashCard>> CreateFlashCardsAsync(IEnumerable<Models.FlashCard> flashCards);
    Task<(IEnumerable<Models.FlashCard> results, int createdCount, int updatedCount)> BulkCreateOrUpdateFlashCardsAsync(int collectionId, IEnumerable<FlashCardItem> flashCardItems);
    Task<Models.FlashCard?> UpdateFlashCardAsync(int id, Models.FlashCard flashCard);
    Task<bool> DeleteFlashCardAsync(int id);
    Task<(int deletedCount, int failedCount)> BulkDeleteFlashCardsAsync(IEnumerable<int> flashCardIds);
}
