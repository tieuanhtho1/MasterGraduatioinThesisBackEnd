using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.BusinessLogic.Auth;
using WebAPI.Models.DTOs;
using WebAPI.Services;

namespace WebAPI.Controllers.Auth;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthBusinessLogic _authBusinessLogic;

    public AuthController(IAuthBusinessLogic authBusinessLogic)
    {
        _authBusinessLogic = authBusinessLogic;
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || 
            string.IsNullOrWhiteSpace(request.Email) || 
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new { message = "All fields are required" });
        }

        var result = await _authBusinessLogic.RegisterAsync(request);
        
        if (result == null)
        {
            return BadRequest(new { message = "Username or email already exists" });
        }

        return Ok(result);
    }

    /// <summary>
    /// Login with username and password
    /// </summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new { message = "Username and password are required" });
        }

        var result = await _authBusinessLogic.LoginAsync(request);
        
        if (result == null)
        {
            return BadRequest(new { message = "Invalid username or password" });
        }

        return Ok(result);
    }

    /// <summary>
    /// Logout (requires authentication)
    /// </summary>
    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        // In JWT, logout is typically handled client-side by removing the token
        // You can implement token blacklisting here if needed
        return Ok(new { message = "Logged out successfully" });
    }
}
