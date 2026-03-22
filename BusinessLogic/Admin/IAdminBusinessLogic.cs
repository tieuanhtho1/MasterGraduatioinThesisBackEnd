using WebAPI.Models.DTOs.Admin;

namespace WebAPI.BusinessLogic.Admin;

public interface IAdminBusinessLogic
{
    // User management
    Task<List<AdminUserResponse>> GetAllUsersAsync();
    Task<AdminUserResponse?> GetUserByIdAsync(int id);
    Task<AdminUserResponse?> UpdateUserAsync(int id, AdminUpdateUserRequest request);
    Task<bool> DeleteUserAsync(int id);

    // UserApiKey management
    Task<List<AdminUserApiKeyResponse>> GetAllApiKeysAsync();
    Task<List<AdminUserApiKeyResponse>> GetApiKeysByUserIdAsync(int userId);
    Task<AdminUserApiKeyResponse?> GetApiKeyByIdAsync(int id);
    Task<AdminUserApiKeyResponse> CreateApiKeyAsync(AdminCreateUserApiKeyRequest request);
    Task<AdminUserApiKeyResponse?> UpdateApiKeyAsync(int id, AdminUpdateUserApiKeyRequest request);
    Task<bool> DeleteApiKeyAsync(int id);
}
