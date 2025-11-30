using WebAPI.Models;

namespace WebAPI.Services.FlashCardCollection;

public interface IFlashCardCollectionService
{
    Task<Models.FlashCardCollection?> GetCollectionByIdAsync(int id);
    Task<IEnumerable<Models.FlashCardCollection>> GetCollectionsByUserIdAsync(int userId);
    Task<IEnumerable<Models.FlashCardCollection>> GetAllCollectionsAsync();
    Task<Models.FlashCardCollection> CreateCollectionAsync(Models.FlashCardCollection collection);
    Task<Models.FlashCardCollection> UpdateCollectionAsync(Models.FlashCardCollection collection);
    Task<bool> DeleteCollectionAsync(int id);
}
