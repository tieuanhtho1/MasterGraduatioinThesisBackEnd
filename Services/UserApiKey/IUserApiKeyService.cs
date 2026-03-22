using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Services.UserApiKey;

public interface IUserApiKeyService
{
    // User-scoped queries
    Task<List<Models.UserApiKey>> GetByUserIdAsync(int userId);
    Task<Models.UserApiKey?> GetByUserAndProviderAsync(int userId, string provider);

    // Admin-scoped queries
    Task<List<Models.UserApiKey>> GetAllAsync();
    Task<Models.UserApiKey?> GetByIdAsync(int id);

    // Mutations
    Task<Models.UserApiKey> CreateAsync(Models.UserApiKey entity);
    Task<Models.UserApiKey?> UpdateAsync(Models.UserApiKey entity);
    Task<bool> DeleteAsync(int id);
}
