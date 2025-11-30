using WebAPI.Models.Auth.DTOs;
using WebAPI.Models.DTOs;

namespace WebAPI.BusinessLogic.Auth;

public class AuthBusinessLogic : IAuthBusinessLogic
{
    private readonly Services.IAuthService _authService;

    public AuthBusinessLogic(Services.IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<AuthResponse?> RegisterAsync(RegisterRequest request)
    {
        // Business logic validations can be added here
        // For example: password strength requirements, username format validation, etc.
        
        return await _authService.RegisterAsync(request);
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        // Business logic can be added here
        // For example: rate limiting, account lockout after failed attempts, etc.
        
        return await _authService.LoginAsync(request);
    }
}
