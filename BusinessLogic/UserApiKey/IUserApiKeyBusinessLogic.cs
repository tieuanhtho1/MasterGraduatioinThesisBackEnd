using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPI.Models.DTOs.UserApiKey;

namespace WebAPI.BusinessLogic.UserApiKey;

public interface IUserApiKeyBusinessLogic
{
    Task<List<UserApiKeyResponse>> GetByUserIdAsync(int userId);
    Task<UserApiKeyResponse?> GetByUserAndProviderAsync(int userId, string provider);
    Task<UserApiKeyResponse> CreateAsync(int userId, CreateUserApiKeyRequest request);
    Task<UserApiKeyResponse?> UpdateAsync(int userId, int id, UpdateUserApiKeyRequest request);
    Task<bool> DeleteAsync(int userId, int id);
    Task<string> GetApiKeyForProviderAsync(int userId, string provider);
}
