using WebAPI.Models.DTOs.Admin;
using WebAPI.Services.User;
using WebAPI.Services.UserApiKey;

namespace WebAPI.BusinessLogic.Admin;

public class AdminBusinessLogic : IAdminBusinessLogic
{
    private readonly IUserService _userService;
    private readonly IUserApiKeyService _apiKeyService;

    public AdminBusinessLogic(IUserService userService, IUserApiKeyService apiKeyService)
    {
        _userService = userService;
        _apiKeyService = apiKeyService;
    }

    // ─── User Management ────────────────────────────────────────────────────────

    public async Task<List<AdminUserResponse>> GetAllUsersAsync()
    {
        var users = await _userService.GetAllUsersAsync();
        return users.Select(u => MapUserToResponse(u)).ToList();
    }

    public async Task<AdminUserResponse?> GetUserByIdAsync(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);
        return user == null ? null : MapUserToResponse(user);
    }

    public async Task<AdminUserResponse?> UpdateUserAsync(int id, AdminUpdateUserRequest request)
    {
        var user = await _userService.GetUserByIdAsync(id);
        if (user == null) return null;

        if (!string.IsNullOrWhiteSpace(request.Username))
            user.Username = request.Username;

        if (!string.IsNullOrWhiteSpace(request.Email))
            user.Email = request.Email;

        if (request.Role.HasValue)
            user.Role = request.Role.Value;

        var updated = await _userService.UpdateUserAsync(user);
        return MapUserToResponse(updated);
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        return await _userService.DeleteUserAsync(id);
    }

    // ─── UserApiKey Management ───────────────────────────────────────────────────

    public async Task<List<AdminUserApiKeyResponse>> GetAllApiKeysAsync()
    {
        var keys = await _apiKeyService.GetAllAsync();
        return keys.Select(k => MapApiKeyToResponse(k)).ToList();
    }

    public async Task<List<AdminUserApiKeyResponse>> GetApiKeysByUserIdAsync(int userId)
    {
        var keys = await _apiKeyService.GetByUserIdAsync(userId);
        var user = await _userService.GetUserByIdAsync(userId);
        var username = user?.Username ?? string.Empty;
        return keys.Select(k => MapApiKeyToResponse(k, username)).ToList();
    }

    public async Task<AdminUserApiKeyResponse?> GetApiKeyByIdAsync(int id)
    {
        var key = await _apiKeyService.GetByIdAsync(id);
        return key == null ? null : MapApiKeyToResponse(key);
    }

    public async Task<AdminUserApiKeyResponse> CreateApiKeyAsync(AdminCreateUserApiKeyRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Provider))
            throw new ArgumentException("Provider is required.");
        if (string.IsNullOrWhiteSpace(request.ApiKey))
            throw new ArgumentException("ApiKey is required.");

        var user = await _userService.GetUserByIdAsync(request.UserId);
        if (user == null)
            throw new ArgumentException($"User with ID {request.UserId} not found.");

        var existing = await _apiKeyService.GetByUserAndProviderAsync(request.UserId, request.Provider);
        if (existing != null)
            throw new InvalidOperationException(
                $"An API key for provider '{request.Provider}' already exists for user {request.UserId}. Use update instead.");

        var entity = new Models.UserApiKey
        {
            UserId = request.UserId,
            Provider = request.Provider,
            ApiKey = request.ApiKey,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _apiKeyService.CreateAsync(entity);
        // Attach user so mapper can include username
        created.User = user;
        return MapApiKeyToResponse(created);
    }

    public async Task<AdminUserApiKeyResponse?> UpdateApiKeyAsync(int id, AdminUpdateUserApiKeyRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Provider))
            throw new ArgumentException("Provider is required.");
        if (string.IsNullOrWhiteSpace(request.ApiKey))
            throw new ArgumentException("ApiKey is required.");

        var existing = await _apiKeyService.GetByIdAsync(id);
        if (existing == null) return null;

        var entity = new Models.UserApiKey
        {
            Id = id,
            UserId = existing.UserId,
            Provider = request.Provider,
            ApiKey = request.ApiKey
        };

        var updated = await _apiKeyService.UpdateAsync(entity);
        if (updated == null) return null;

        // Re-fetch with user navigation included
        var withUser = await _apiKeyService.GetByIdAsync(updated.Id);
        return withUser == null ? null : MapApiKeyToResponse(withUser);
    }

    public async Task<bool> DeleteApiKeyAsync(int id)
    {
        return await _apiKeyService.DeleteAsync(id);
    }

    // ─── Mappers ─────────────────────────────────────────────────────────────────

    private static AdminUserResponse MapUserToResponse(Models.User user) => new()
    {
        Id = user.Id,
        Username = user.Username,
        Email = user.Email,
        Role = user.Role.ToString(),
        CreatedAt = user.CreatedAt,
        LastLoginAt = user.LastLoginAt
    };

    private static AdminUserApiKeyResponse MapApiKeyToResponse(Models.UserApiKey key, string? overrideUsername = null) => new()
    {
        Id = key.Id,
        UserId = key.UserId,
        Username = overrideUsername ?? key.User?.Username ?? string.Empty,
        Provider = key.Provider,
        ApiKey = key.ApiKey,
        CreatedAt = key.CreatedAt,
        UpdatedAt = key.UpdatedAt
    };
}
