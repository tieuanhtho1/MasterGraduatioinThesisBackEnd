using WebAPI.Models;
using WebAPI.Services.User;

namespace WebAPI.BusinessLogic.User;

public class UserBusinessLogic : IUserBusinessLogic
{
    private readonly IUserService _userService;

    public UserBusinessLogic(IUserService userService)
    {
        _userService = userService;
    }

    public async Task<Models.User?> GetUserByIdAsync(int id)
    {
        return await _userService.GetUserByIdAsync(id);
    }

    public async Task<IEnumerable<Models.User>> GetAllUsersAsync()
    {
        return await _userService.GetAllUsersAsync();
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        // Business logic: Check if user exists and can be deleted
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return false;
        }

        // Additional business rules can be added here
        // For example: prevent deleting users with active sessions, etc.

        return await _userService.DeleteUserAsync(id);
    }

    public async Task<Models.User?> UpdateUserRoleAsync(int id, UserRole role)
    {
        // Business logic: Validate role change
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null)
        {
            return null;
        }

        // Additional business rules can be added here
        // For example: prevent role changes for certain users, log changes, etc.

        user.Role = role;
        return await _userService.UpdateUserAsync(user);
    }
}
