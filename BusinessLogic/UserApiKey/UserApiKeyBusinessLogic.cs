using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models.DTOs.UserApiKey;
using WebAPI.Services.UserApiKey;

namespace WebAPI.BusinessLogic.UserApiKey;

public class UserApiKeyBusinessLogic : IUserApiKeyBusinessLogic
{
    private readonly IUserApiKeyService _service;

    public UserApiKeyBusinessLogic(IUserApiKeyService service)
    {
        _service = service;
    }

    public async Task<List<UserApiKeyResponse>> GetByUserIdAsync(int userId)
    {
        var keys = await _service.GetByUserIdAsync(userId);
        return keys.Select(MapToResponse).ToList();
    }

    public async Task<UserApiKeyResponse?> GetByUserAndProviderAsync(int userId, string provider)
    {
        var key = await _service.GetByUserAndProviderAsync(userId, provider);
        return key == null ? null : MapToResponse(key);
    }

    public async Task<UserApiKeyResponse> CreateAsync(int userId, CreateUserApiKeyRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Provider))
            throw new ArgumentException("Provider is required");
        if (string.IsNullOrWhiteSpace(request.ApiKey))
            throw new ArgumentException("ApiKey is required");

        // Check if a key for this provider already exists
        var existing = await _service.GetByUserAndProviderAsync(userId, request.Provider);
        if (existing != null)
            throw new InvalidOperationException($"An API key for provider '{request.Provider}' already exists. Use update instead.");

        var entity = new Models.UserApiKey
        {
            UserId = userId,
            Provider = request.Provider,
            ApiKey = request.ApiKey,
            CreatedAt = DateTime.UtcNow
        };

        var created = await _service.CreateAsync(entity);
        return MapToResponse(created);
    }

    public async Task<UserApiKeyResponse?> UpdateAsync(int userId, int id, UpdateUserApiKeyRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Provider))
            throw new ArgumentException("Provider is required");
        if (string.IsNullOrWhiteSpace(request.ApiKey))
            throw new ArgumentException("ApiKey is required");

        var entity = new Models.UserApiKey
        {
            Id = id,
            UserId = userId,
            Provider = request.Provider,
            ApiKey = request.ApiKey
        };

        var updated = await _service.UpdateAsync(entity);
        return updated == null ? null : MapToResponse(updated);
    }

    public async Task<bool> DeleteAsync(int userId, int id)
    {
        // We could add ownership check here if needed
        return await _service.DeleteAsync(id);
    }

    public async Task<string> GetApiKeyForProviderAsync(int userId, string provider)
    {
        var key = await _service.GetByUserAndProviderAsync(userId, provider);
        if (key == null)
            throw new KeyNotFoundException(
                $"No API key found for provider '{provider}'. Please add your API key in settings.");

        return key.ApiKey;
    }

    private static UserApiKeyResponse MapToResponse(Models.UserApiKey entity)
    {
        return new UserApiKeyResponse
        {
            Id = entity.Id,
            Provider = entity.Provider,
            ApiKey = entity.ApiKey,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };
    }
}
