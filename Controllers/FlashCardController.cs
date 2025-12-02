using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.BusinessLogic.FlashCard;
using WebAPI.Models.DTOs.FlashCard;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlashCardController : ControllerBase
{
    private readonly IFlashCardBusinessLogic _flashCardBusinessLogic;

    public FlashCardController(IFlashCardBusinessLogic flashCardBusinessLogic)
    {
        _flashCardBusinessLogic = flashCardBusinessLogic;
    }

    /// <summary>
    /// Get a flashcard by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetFlashCard(int id)
    {
        var flashCard = await _flashCardBusinessLogic.GetFlashCardByIdAsync(id);
        
        if (flashCard == null)
        {
            return NotFound(new { message = "FlashCard not found" });
        }

        var response = new FlashCardResponse
        {
            Id = flashCard.Id,
            Term = flashCard.Term,
            Definition = flashCard.Definition,
            Score = flashCard.Score,
            FlashCardCollectionId = flashCard.FlashCardCollectionId
        };

        return Ok(response);
    }

    /// <summary>
    /// Get all flashcards in a collection
    /// </summary>
    [HttpGet("collection/{collectionId}")]
    public async Task<IActionResult> GetFlashCardsByCollection(int collectionId)
    {
        var flashCards = await _flashCardBusinessLogic.GetFlashCardsByCollectionIdAsync(collectionId);
        
        var response = flashCards.Select(fc => new FlashCardResponse
        {
            Id = fc.Id,
            Term = fc.Term,
            Definition = fc.Definition,
            Score = fc.Score,
            FlashCardCollectionId = fc.FlashCardCollectionId
        });

        return Ok(response);
    }

    /// <summary>
    /// Create a new flashcard
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateFlashCard([FromBody] CreateFlashCardRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Term) || string.IsNullOrWhiteSpace(request.Definition))
        {
            return BadRequest(new { message = "Term and Definition are required" });
        }

        if (request.FlashCardCollectionId <= 0)
        {
            return BadRequest(new { message = "Valid FlashCardCollectionId is required" });
        }

        var flashCard = new Models.FlashCard
        {
            Term = request.Term,
            Definition = request.Definition,
            Score = request.Score,
            FlashCardCollectionId = request.FlashCardCollectionId
        };

        var result = await _flashCardBusinessLogic.CreateFlashCardAsync(flashCard);
        
        if (result == null)
        {
            return BadRequest(new { message = "Failed to create flashcard" });
        }

        var response = new FlashCardResponse
        {
            Id = result.Id,
            Term = result.Term,
            Definition = result.Definition,
            Score = result.Score,
            FlashCardCollectionId = result.FlashCardCollectionId
        };

        return CreatedAtAction(nameof(GetFlashCard), new { id = result.Id }, response);
    }

    /// <summary>
    /// Create multiple flashcards at once
    /// </summary>
    [HttpPost("bulk")]
    public async Task<IActionResult> CreateFlashCardsBulk([FromBody] BulkCreateFlashCardsRequest request)
    {
        if (request.FlashCardCollectionId <= 0)
        {
            return BadRequest(new { message = "Valid FlashCardCollectionId is required" });
        }

        if (request.FlashCards == null || !request.FlashCards.Any())
        {
            return BadRequest(new { message = "At least one flashcard is required" });
        }

        var flashCards = request.FlashCards.Select(fc => new Models.FlashCard
        {
            Term = fc.Term,
            Definition = fc.Definition,
            Score = fc.Score,
            FlashCardCollectionId = request.FlashCardCollectionId
        });

        var results = await _flashCardBusinessLogic.CreateFlashCardsAsync(flashCards);
        
        if (!results.Any())
        {
            return BadRequest(new { message = "Failed to create flashcards. Ensure all flashcards have valid Term and Definition" });
        }

        var response = results.Select(fc => new FlashCardResponse
        {
            Id = fc.Id,
            Term = fc.Term,
            Definition = fc.Definition,
            Score = fc.Score,
            FlashCardCollectionId = fc.FlashCardCollectionId
        });

        return Ok(new 
        { 
            message = $"Successfully created {results.Count()} flashcard(s)",
            flashCards = response 
        });
    }

    /// <summary>
    /// Update a flashcard
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFlashCard(int id, [FromBody] UpdateFlashCardRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Term) || string.IsNullOrWhiteSpace(request.Definition))
        {
            return BadRequest(new { message = "Term and Definition are required" });
        }

        var flashCard = new Models.FlashCard
        {
            Term = request.Term,
            Definition = request.Definition,
            Score = request.Score
        };

        var result = await _flashCardBusinessLogic.UpdateFlashCardAsync(id, flashCard);
        
        if (result == null)
        {
            return NotFound(new { message = "FlashCard not found" });
        }

        var response = new FlashCardResponse
        {
            Id = result.Id,
            Term = result.Term,
            Definition = result.Definition,
            Score = result.Score,
            FlashCardCollectionId = result.FlashCardCollectionId
        };

        return Ok(response);
    }

    /// <summary>
    /// Delete a flashcard
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFlashCard(int id)
    {
        var result = await _flashCardBusinessLogic.DeleteFlashCardAsync(id);
        
        if (!result)
        {
            return NotFound(new { message = "FlashCard not found" });
        }

        return Ok(new { message = "FlashCard deleted successfully" });
    }
}
