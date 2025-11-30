using WebAPI.Models;

namespace WebAPI.BusinessLogic.FlashCard;

public interface IFlashCardBusinessLogic
{
    Task<Models.FlashCard?> GetFlashCardByIdAsync(int id);
    Task<IEnumerable<Models.FlashCard>> GetFlashCardsByCollectionIdAsync(int collectionId);
    Task<Models.FlashCard?> CreateFlashCardAsync(Models.FlashCard flashCard);
    Task<Models.FlashCard?> UpdateFlashCardAsync(int id, Models.FlashCard flashCard);
    Task<bool> DeleteFlashCardAsync(int id);
}
