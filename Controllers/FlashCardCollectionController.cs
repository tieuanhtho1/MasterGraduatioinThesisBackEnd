using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.BusinessLogic.FlashCardCollection;
using WebAPI.Models.DTOs.FlashCardCollection;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlashCardCollectionController : ControllerBase
{
    private readonly IFlashCardCollectionBusinessLogic _collectionBusinessLogic;

    public FlashCardCollectionController(IFlashCardCollectionBusinessLogic collectionBusinessLogic)
    {
        _collectionBusinessLogic = collectionBusinessLogic;
    }

    /// <summary>
    /// Get a collection by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCollection(int id)
    {
        var collection = await _collectionBusinessLogic.GetCollectionByIdAsync(id);
        
        if (collection == null)
        {
            return NotFound(new { message = "Collection not found" });
        }

        var response = new FlashCardCollectionResponse
        {
            Id = collection.Id,
            UserId = collection.UserId,
            ParentId = collection.ParentId,
            Title = collection.Title,
            Description = collection.Description,
            FlashCardCount = collection.FlashCards?.Count ?? 0,
            ChildrenCount = collection.Children?.Count ?? 0
        };

        return Ok(response);
    }

    /// <summary>
    /// Get all collections for a specific user
    /// </summary>
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetCollectionsByUser(int userId)
    {
        var collections = await _collectionBusinessLogic.GetCollectionsByUserIdAsync(userId);
        
        var response = collections.Select(c => new FlashCardCollectionResponse
        {
            Id = c.Id,
            UserId = c.UserId,
            ParentId = c.ParentId,
            Title = c.Title,
            Description = c.Description,
            FlashCardCount = c.FlashCards?.Count ?? 0,
            ChildrenCount = c.Children?.Count ?? 0
        });

        return Ok(response);
    }

    /// <summary>
    /// Get all child collections (sub-collections) of a parent collection
    /// </summary>
    [HttpGet("{id}/children")]
    public async Task<IActionResult> GetChildCollections(int id)
    {
        var children = await _collectionBusinessLogic.GetChildrenByParentIdAsync(id);
        
        var response = children.Select(c => new FlashCardCollectionResponse
        {
            Id = c.Id,
            UserId = c.UserId,
            ParentId = c.ParentId,
            Title = c.Title,
            Description = c.Description,
            FlashCardCount = c.FlashCards?.Count ?? 0,
            ChildrenCount = c.Children?.Count ?? 0
        });

        return Ok(response);
    }

    /// <summary>
    /// Create a new collection
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateCollection([FromBody] CreateFlashCardCollectionRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return BadRequest(new { message = "Title is required" });
        }

        if (request.UserId <= 0)
        {
            return BadRequest(new { message = "Valid UserId is required" });
        }

        var collection = new Models.FlashCardCollection
        {
            UserId = request.UserId,
            ParentId = request.ParentId,
            Title = request.Title,
            Description = request.Description
        };

        var result = await _collectionBusinessLogic.CreateCollectionAsync(collection);
        
        if (result == null)
        {
            return BadRequest(new { message = "Failed to create collection" });
        }

        var response = new FlashCardCollectionResponse
        {
            Id = result.Id,
            UserId = result.UserId,
            ParentId = result.ParentId,
            Title = result.Title,
            Description = result.Description,
            FlashCardCount = result.FlashCards?.Count ?? 0,
            ChildrenCount = result.Children?.Count ?? 0
        };

        return CreatedAtAction(nameof(GetCollection), new { id = result.Id }, response);
    }

    /// <summary>
    /// Update a collection
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCollection(int id, [FromBody] UpdateFlashCardCollectionRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
        {
            return BadRequest(new { message = "Title is required" });
        }

        var collection = new Models.FlashCardCollection
        {
            Title = request.Title,
            Description = request.Description,
            ParentId = request.ParentId
        };

        var result = await _collectionBusinessLogic.UpdateCollectionAsync(id, collection);
        
        if (result == null)
        {
            return NotFound(new { message = "Collection not found" });
        }

        var response = new FlashCardCollectionResponse
        {
            Id = result.Id,
            UserId = result.UserId,
            ParentId = result.ParentId,
            Title = result.Title,
            Description = result.Description,
            FlashCardCount = result.FlashCards?.Count ?? 0,
            ChildrenCount = result.Children?.Count ?? 0
        };

        return Ok(response);
    }

    /// <summary>
    /// Delete a collection
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCollection(int id)
    {
        var result = await _collectionBusinessLogic.DeleteCollectionAsync(id);
        
        if (!result)
        {
            return NotFound(new { message = "Collection not found" });
        }

        return Ok(new { message = "Collection deleted successfully" });
    }
}
