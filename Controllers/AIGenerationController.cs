using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPI.BusinessLogic.AIGeneration;
using WebAPI.Models.DTOs.AIGeneration;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AIGenerationController : ControllerBase
{
    private readonly IAIGenerationBusinessLogic _aiLogic;

    public AIGenerationController(IAIGenerationBusinessLogic aiLogic)
    {
        _aiLogic = aiLogic;
    }

    [HttpPost("flashcards")]
    public async Task<IActionResult> GenerateFlashCards([FromForm] GenerateFlashCardsRequest request)
    {
        var response = await _aiLogic.GenerateFlashCardsAsync(request);
        return Ok(response);
    }

    [HttpPost("mindmap")]
    public async Task<IActionResult> GenerateMindMap([FromBody] GenerateMindMapRequest request)
    {
        var response = await _aiLogic.GenerateMindMapAsync(request);
        return Ok(response);
    }

    [HttpPost("all")]
    public async Task<IActionResult> GenerateAll([FromForm] GenerateAllRequest request)
    {
        var response = await _aiLogic.GenerateAllAsync(request);
        return Ok(response);
    }
}
