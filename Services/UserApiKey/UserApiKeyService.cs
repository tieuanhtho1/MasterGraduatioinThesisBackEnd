using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;

namespace WebAPI.Services.UserApiKey;

public class UserApiKeyService : IUserApiKeyService
{
    private readonly ApplicationDbContext _context;

    public UserApiKeyService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Models.UserApiKey>> GetByUserIdAsync(int userId)
    {
        return await _context.UserApiKeys
            .Where(k => k.UserId == userId)
            .OrderBy(k => k.Provider)
            .ToListAsync();
    }

    public async Task<List<Models.UserApiKey>> GetAllAsync()
    {
        return await _context.UserApiKeys
            .Include(k => k.User)
            .OrderBy(k => k.UserId)
            .ThenBy(k => k.Provider)
            .ToListAsync();
    }

    public async Task<Models.UserApiKey?> GetByIdAsync(int id)
    {
        return await _context.UserApiKeys
            .Include(k => k.User)
            .FirstOrDefaultAsync(k => k.Id == id);
    }

    public async Task<Models.UserApiKey?> GetByUserAndProviderAsync(int userId, string provider)
    {
        return await _context.UserApiKeys
            .FirstOrDefaultAsync(k => k.UserId == userId
                && k.Provider.ToLower() == provider.ToLower());
    }

    public async Task<Models.UserApiKey> CreateAsync(Models.UserApiKey entity)
    {
        _context.UserApiKeys.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Models.UserApiKey?> UpdateAsync(Models.UserApiKey entity)
    {
        var existing = await _context.UserApiKeys.FindAsync(entity.Id);
        if (existing == null) return null;

        existing.Provider = entity.Provider;
        existing.ApiKey = entity.ApiKey;
        existing.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var entity = await _context.UserApiKeys.FindAsync(id);
        if (entity == null) return false;

        _context.UserApiKeys.Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}
