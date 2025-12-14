using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.BusinessLogic.LearnSession;
using WebAPI.Models.DTOs.LearnSession;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LearnSessionController : ControllerBase
{
    private readonly ILearnSessionBusinessLogic _learnSessionBusinessLogic;

    public LearnSessionController(ILearnSessionBusinessLogic learnSessionBusinessLogic)
    {
        _learnSessionBusinessLogic = learnSessionBusinessLogic;
    }

    /// <summary>
    /// Get a learn session with randomly selected flashcards from a collection and its children
    /// </summary>
    /// <param name="collectionId">The root collection ID</param>
    /// <param name="count">Number of flashcards to return (default: 10)</param>
    /// <returns>List of randomly selected flashcards weighted by score</returns>
    [HttpGet]
    public async Task<IActionResult> GetLearnSession([FromQuery] int collectionId, [FromQuery] int count = 10)
    {
        if (collectionId <= 0)
        {
            return BadRequest(new { message = "Valid collection ID is required" });
        }

        if (count <= 0 || count > 50)
        {
            return BadRequest(new { message = "Count must be between 1 and 50" });
        }

        var flashCards = await _learnSessionBusinessLogic.GetLearnSessionFlashCardsAsync(collectionId, count);

        if (!flashCards.Any())
        {
            return NotFound(new { message = "No flashcards found in this collection tree" });
        }

        var response = flashCards.Select(fc => new LearnSessionFlashCardResponse
        {
            Id = fc.Id,
            Term = fc.Term,
            Definition = fc.Definition,
            Score = fc.Score,
            FlashCardCollectionId = fc.FlashCardCollectionId
        });

        return Ok(new
        {
            collectionId,
            count = flashCards.Count,
            flashCards = response
        });
    }

    /// <summary>
    /// Update scores for multiple flashcards
    /// </summary>
    /// <param name="request">List of flashcard IDs with their score modifications</param>
    /// <returns>Success status</returns>
    [HttpPut("scores")]
    public async Task<IActionResult> UpdateScores([FromBody] UpdateScoreRequest request)
    {
        if (request.ScoreUpdates == null || !request.ScoreUpdates.Any())
        {
            return BadRequest(new { message = "At least one score update is required" });
        }

        // Validate all flashcard IDs
        if (request.ScoreUpdates.Any(su => su.FlashCardId <= 0))
        {
            return BadRequest(new { message = "All flashcard IDs must be valid" });
        }

        // Convert to dictionary
        var scoreModifications = request.ScoreUpdates.ToDictionary(
            su => su.FlashCardId,
            su => su.ScoreModification
        );

        var result = await _learnSessionBusinessLogic.UpdateFlashCardScoresAsync(scoreModifications);

        if (!result)
        {
            return BadRequest(new { message = "Failed to update scores" });
        }

        return Ok(new
        {
            message = "Scores updated successfully",
            updatedCount = request.ScoreUpdates.Count
        });
    }
}
