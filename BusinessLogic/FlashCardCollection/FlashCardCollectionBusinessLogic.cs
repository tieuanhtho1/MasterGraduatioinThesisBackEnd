using WebAPI.Services.FlashCardCollection;

namespace WebAPI.BusinessLogic.FlashCardCollection;

public class FlashCardCollectionBusinessLogic : IFlashCardCollectionBusinessLogic
{
    private readonly IFlashCardCollectionService _collectionService;

    public FlashCardCollectionBusinessLogic(IFlashCardCollectionService collectionService)
    {
        _collectionService = collectionService;
    }

    public async Task<Models.FlashCardCollection?> GetCollectionByIdAsync(int id)
    {
        return await _collectionService.GetCollectionByIdAsync(id);
    }

    public async Task<IEnumerable<Models.FlashCardCollection>> GetCollectionsByUserIdAsync(int userId)
    {
        return await _collectionService.GetCollectionsByUserIdAsync(userId);
    }

    public async Task<IEnumerable<Models.FlashCardCollection>> GetChildrenByParentIdAsync(int parentId)
    {
        return await _collectionService.GetChildrenByParentIdAsync(parentId);
    }

    public async Task<Models.FlashCardCollection?> CreateCollectionAsync(Models.FlashCardCollection collection)
    {
        // Business logic: Validate collection data
        if (string.IsNullOrWhiteSpace(collection.Title))
        {
            return null;
        }

        // Additional business rules can be added here
        collection.ParentId = (collection.ParentId == 0) ? null : collection.ParentId;
        return await _collectionService.CreateCollectionAsync(collection);
    }

    public async Task<Models.FlashCardCollection?> UpdateCollectionAsync(int id, Models.FlashCardCollection collection)
    {
        // Business logic: Validate update
        var existing = await _collectionService.GetCollectionByIdAsync(id);
        if (existing == null)
        {
            return null;
        }

        // Additional business rules can be added here
        
        existing.Title = collection.Title;
        existing.Description = collection.Description;

        return await _collectionService.UpdateCollectionAsync(existing);
    }

    public async Task<Models.FlashCardCollection?> UpdateCollectionParentAsync(int id, int? parentId)
    {
        var existing = await _collectionService.GetCollectionByIdAsync(id);
        if (existing == null)
        {
            return null;
        }

        // Cannot set a collection as its own parent
        if (parentId.HasValue && parentId.Value == id)
        {
            return null;
        }

        // Validate that the new parent exists
        if (parentId.HasValue)
        {
            var parentCollection = await _collectionService.GetCollectionByIdAsync(parentId.Value);
            if (parentCollection == null)
            {
                return null;
            }

            // Prevent circular reference: the new parent must not be a descendant of this collection
            var currentParentId = parentCollection.ParentId;
            while (currentParentId.HasValue)
            {
                if (currentParentId.Value == id)
                {
                    return null; // Circular reference detected
                }
                var ancestor = await _collectionService.GetCollectionByIdAsync(currentParentId.Value);
                currentParentId = ancestor?.ParentId;
            }
        }

        existing.ParentId = parentId;
        return await _collectionService.UpdateCollectionAsync(existing);
    }

    public async Task<bool> DeleteCollectionAsync(int id)
    {
        // Business logic: Check if collection can be deleted
        var collection = await _collectionService.GetCollectionByIdAsync(id);
        if (collection == null)
        {
            return false;
        }

        // Additional business rules can be added here
        // For example: check if collection has flashcards, cascade delete, etc.
        
        return await _collectionService.DeleteCollectionAsync(id);
    }

    public async Task<int> GetTotalFlashCardCountAsync(int collectionId)
    {
        return await _collectionService.GetTotalFlashCardCountAsync(collectionId);
    }
}
