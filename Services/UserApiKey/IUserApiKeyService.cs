using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Services.UserApiKey;

public interface IUserApiKeyService
{
    Task<List<Models.UserApiKey>> GetByUserIdAsync(int userId);
    Task<Models.UserApiKey?> GetByUserAndProviderAsync(int userId, string provider);
    Task<Models.UserApiKey> CreateAsync(Models.UserApiKey entity);
    Task<Models.UserApiKey?> UpdateAsync(Models.UserApiKey entity);
    Task<bool> DeleteAsync(int id);
}
