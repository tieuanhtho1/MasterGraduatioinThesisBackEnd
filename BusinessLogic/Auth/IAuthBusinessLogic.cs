using WebAPI.Models.Auth.DTOs;
using WebAPI.Models.DTOs;

namespace WebAPI.BusinessLogic.Auth;

public interface IAuthBusinessLogic
{
    Task<AuthResponse?> RegisterAsync(RegisterRequest request);
    Task<AuthResponse?> LoginAsync(LoginRequest request);
}
