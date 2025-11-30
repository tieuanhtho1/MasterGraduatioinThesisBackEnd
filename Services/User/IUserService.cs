using WebAPI.Models;

namespace WebAPI.Services.User;

public interface IUserService
{
    Task<Models.User?> GetUserByIdAsync(int id);
    Task<Models.User?> GetUserByUsernameAsync(string username);
    Task<Models.User?> GetUserByEmailAsync(string email);
    Task<IEnumerable<Models.User>> GetAllUsersAsync();
    Task<Models.User> CreateUserAsync(Models.User user);
    Task<Models.User> UpdateUserAsync(Models.User user);
    Task<bool> DeleteUserAsync(int id);
    Task<bool> UserExistsByUsernameAsync(string username);
    Task<bool> UserExistsByEmailAsync(string email);
    Task SaveChangesAsync();
}
