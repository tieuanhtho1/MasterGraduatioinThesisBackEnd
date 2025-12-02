using WebAPI.Models;

namespace WebAPI.BusinessLogic.FlashCardCollection;

public interface IFlashCardCollectionBusinessLogic
{
    Task<Models.FlashCardCollection?> GetCollectionByIdAsync(int id);
    Task<IEnumerable<Models.FlashCardCollection>> GetCollectionsByUserIdAsync(int userId);
    Task<IEnumerable<Models.FlashCardCollection>> GetChildrenByParentIdAsync(int parentId);
    Task<Models.FlashCardCollection?> CreateCollectionAsync(Models.FlashCardCollection collection);
    Task<Models.FlashCardCollection?> UpdateCollectionAsync(int id, Models.FlashCardCollection collection);
    Task<bool> DeleteCollectionAsync(int id);
}
