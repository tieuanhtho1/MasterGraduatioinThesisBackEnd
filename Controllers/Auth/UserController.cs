using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.BusinessLogic.User;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserBusinessLogic _userBusinessLogic;

    public UserController(IUserBusinessLogic userBusinessLogic)
    {
        _userBusinessLogic = userBusinessLogic;
    }

    /// <summary>
    /// Get current user profile (User and Admin)
    /// </summary>
    [HttpGet("profile")]
    public IActionResult GetProfile()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var username = User.FindFirst(ClaimTypes.Name)?.Value;
        var email = User.FindFirst(ClaimTypes.Email)?.Value;
        var role = User.FindFirst(ClaimTypes.Role)?.Value;

        return Ok(new
        {
            userId,
            username,
            email,
            role,
            message = "This is a protected endpoint"
        });
    }

    /// <summary>
    /// Get all users (Admin only)
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userBusinessLogic.GetAllUsersAsync();
        
        var userDtos = users.Select(u => new
        {
            u.Id,
            u.Username,
            u.Email,
            Role = u.Role.ToString(),
            u.CreatedAt,
            u.LastLoginAt
        });

        return Ok(userDtos);
    }

    /// <summary>
    /// Get user by ID (Admin only)
    /// </summary>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userBusinessLogic.GetUserByIdAsync(id);

        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }

        return Ok(new
        {
            user.Id,
            user.Username,
            user.Email,
            Role = user.Role.ToString(),
            user.CreatedAt,
            user.LastLoginAt
        });
    }

    /// <summary>
    /// Delete user by ID (Admin only)
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var result = await _userBusinessLogic.DeleteUserAsync(id);

        if (!result)
        {
            return NotFound(new { message = "User not found" });
        }

        return Ok(new { message = "User deleted successfully" });
    }

    /// <summary>
    /// Update user role (Admin only)
    /// </summary>
    [HttpPatch("{id}/role")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateUserRole(int id, [FromBody] UpdateRoleRequest request)
    {
        var user = await _userBusinessLogic.UpdateUserRoleAsync(id, request.Role);

        if (user == null)
        {
            return NotFound(new { message = "User not found" });
        }

        return Ok(new 
        { 
            message = "User role updated successfully",
            userId = user.Id,
            username = user.Username,
            role = user.Role.ToString()
        });
    }
}

public class UpdateRoleRequest
{
    public UserRole Role { get; set; }
}
