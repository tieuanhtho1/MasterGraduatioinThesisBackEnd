using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.BusinessLogic.UserApiKey;
using WebAPI.Models.DTOs.UserApiKey;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserApiKeyController : ControllerBase
{
    private readonly IUserApiKeyBusinessLogic _businessLogic;

    public UserApiKeyController(IUserApiKeyBusinessLogic businessLogic)
    {
        _businessLogic = businessLogic;
    }

    private int GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.Parse(userIdClaim!);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId = GetCurrentUserId();
        var keys = await _businessLogic.GetByUserIdAsync(userId);
        return Ok(keys);
    }

    [HttpGet("{provider}")]
    public async Task<IActionResult> GetByProvider(string provider)
    {
        var userId = GetCurrentUserId();
        var key = await _businessLogic.GetByUserAndProviderAsync(userId, provider);
        if (key == null) return NotFound(new { message = $"No API key found for provider '{provider}'" });
        return Ok(key);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserApiKeyRequest request)
    {
        var userId = GetCurrentUserId();
        try
        {
            var result = await _businessLogic.CreateAsync(userId, request);
            return CreatedAtAction(nameof(GetByProvider), new { provider = result.Provider }, result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUserApiKeyRequest request)
    {
        var userId = GetCurrentUserId();
        var result = await _businessLogic.UpdateAsync(userId, id, request);
        if (result == null) return NotFound(new { message = "API key not found" });
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = GetCurrentUserId();
        var deleted = await _businessLogic.DeleteAsync(userId, id);
        if (!deleted) return NotFound(new { message = "API key not found" });
        return Ok(new { message = "API key deleted successfully" });
    }
}
