using WebAPI.Models;

namespace WebAPI.BusinessLogic.User;

public interface IUserBusinessLogic
{
    Task<Models.User?> GetUserByIdAsync(int id);
    Task<IEnumerable<Models.User>> GetAllUsersAsync();
    Task<bool> DeleteUserAsync(int id);
    Task<Models.User?> UpdateUserRoleAsync(int id, UserRole role);
}
