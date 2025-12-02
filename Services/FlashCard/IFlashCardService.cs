using WebAPI.Models;

namespace WebAPI.Services.FlashCard;

public interface IFlashCardService
{
    Task<Models.FlashCard?> GetFlashCardByIdAsync(int id);
    Task<IEnumerable<Models.FlashCard>> GetFlashCardsByCollectionIdAsync(int collectionId);
    Task<IEnumerable<Models.FlashCard>> GetAllFlashCardsAsync();
    Task<Models.FlashCard> CreateFlashCardAsync(Models.FlashCard flashCard);
    Task<IEnumerable<Models.FlashCard>> CreateFlashCardsAsync(IEnumerable<Models.FlashCard> flashCards);
    Task<Models.FlashCard> UpdateFlashCardAsync(Models.FlashCard flashCard);
    Task<bool> DeleteFlashCardAsync(int id);
}
