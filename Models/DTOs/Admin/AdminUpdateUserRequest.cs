using WebAPI.Models;

namespace WebAPI.Models.DTOs.Admin;

public class AdminUpdateUserRequest
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public UserRole? Role { get; set; }
}
