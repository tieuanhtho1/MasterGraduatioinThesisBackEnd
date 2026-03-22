using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.BusinessLogic.Admin;
using WebAPI.Models.DTOs.Admin;

namespace WebAPI.Controllers.Admin;

/// <summary>
/// Admin-only endpoints for managing users and their API keys.
/// All endpoints require the "Admin" role.
/// </summary>
[ApiController]
[Route("api/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminBusinessLogic _adminBusinessLogic;

    public AdminController(IAdminBusinessLogic adminBusinessLogic)
    {
        _adminBusinessLogic = adminBusinessLogic;
    }

    // ─────────────────────────────────────────────────────────────────────────────
    // User Management
    // ─────────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Get all registered users.
    /// </summary>
    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _adminBusinessLogic.GetAllUsersAsync();
        return Ok(users);
    }

    /// <summary>
    /// Get a specific user by ID.
    /// </summary>
    [HttpGet("users/{id:int}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _adminBusinessLogic.GetUserByIdAsync(id);
        if (user == null)
            return NotFound(new { message = $"User with ID {id} not found." });
        return Ok(user);
    }

    /// <summary>
    /// Update a user's username, email, or role (all fields optional).
    /// </summary>
    [HttpPut("users/{id:int}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] AdminUpdateUserRequest request)
    {
        try
        {
            var user = await _adminBusinessLogic.UpdateUserAsync(id, request);
            if (user == null)
                return NotFound(new { message = $"User with ID {id} not found." });
            return Ok(user);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Delete a user by ID (also cascades their data per DB rules).
    /// </summary>
    [HttpDelete("users/{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var deleted = await _adminBusinessLogic.DeleteUserAsync(id);
        if (!deleted)
            return NotFound(new { message = $"User with ID {id} not found." });
        return Ok(new { message = "User deleted successfully." });
    }

    // ─────────────────────────────────────────────────────────────────────────────
    // UserApiKey Management
    // ─────────────────────────────────────────────────────────────────────────────

    /// <summary>
    /// Get all API keys across all users.
    /// </summary>
    [HttpGet("apikeys")]
    public async Task<IActionResult> GetAllApiKeys()
    {
        var keys = await _adminBusinessLogic.GetAllApiKeysAsync();
        return Ok(keys);
    }

    /// <summary>
    /// Get all API keys belonging to a specific user.
    /// </summary>
    [HttpGet("users/{userId:int}/apikeys")]
    public async Task<IActionResult> GetApiKeysByUser(int userId)
    {
        var keys = await _adminBusinessLogic.GetApiKeysByUserIdAsync(userId);
        return Ok(keys);
    }

    /// <summary>
    /// Get a single API key by its ID.
    /// </summary>
    [HttpGet("apikeys/{id:int}")]
    public async Task<IActionResult> GetApiKeyById(int id)
    {
        var key = await _adminBusinessLogic.GetApiKeyByIdAsync(id);
        if (key == null)
            return NotFound(new { message = $"API key with ID {id} not found." });
        return Ok(key);
    }

    /// <summary>
    /// Create a new API key for any user.
    /// </summary>
    [HttpPost("apikeys")]
    public async Task<IActionResult> CreateApiKey([FromBody] AdminCreateUserApiKeyRequest request)
    {
        try
        {
            var result = await _adminBusinessLogic.CreateApiKeyAsync(request);
            return CreatedAtAction(nameof(GetApiKeyById), new { id = result.Id }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing API key's provider and key value.
    /// </summary>
    [HttpPut("apikeys/{id:int}")]
    public async Task<IActionResult> UpdateApiKey(int id, [FromBody] AdminUpdateUserApiKeyRequest request)
    {
        try
        {
            var result = await _adminBusinessLogic.UpdateApiKeyAsync(id, request);
            if (result == null)
                return NotFound(new { message = $"API key with ID {id} not found." });
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    /// <summary>
    /// Delete an API key by ID.
    /// </summary>
    [HttpDelete("apikeys/{id:int}")]
    public async Task<IActionResult> DeleteApiKey(int id)
    {
        var deleted = await _adminBusinessLogic.DeleteApiKeyAsync(id);
        if (!deleted)
            return NotFound(new { message = $"API key with ID {id} not found." });
        return Ok(new { message = "API key deleted successfully." });
    }
}
