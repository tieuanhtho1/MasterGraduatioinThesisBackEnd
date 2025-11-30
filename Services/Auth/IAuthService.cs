using WebAPI.Models;
using WebAPI.Models.Auth.DTOs;
using WebAPI.Models.DTOs;

namespace WebAPI.Services;

public interface IAuthService
{
    Task<AuthResponse?> RegisterAsync(RegisterRequest request);
    Task<AuthResponse?> LoginAsync(LoginRequest request);
    string GenerateJwtToken(Models.User user);
    string HashPassword(string password);
    bool VerifyPassword(string password, string passwordHash);
}
