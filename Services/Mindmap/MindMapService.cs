using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Services.MindMap;

public class MindMapService : IMindMapService
{
    private readonly ApplicationDbContext _context;

    public MindMapService(ApplicationDbContext context)
    {
        _context = context;
    }

    // ──────────────── MindMap ────────────────

    public async Task<Models.MindMap?> GetMindMapByIdAsync(int id)
    {
        return await _context.MindMaps
            .Include(m => m.FlashCardCollection)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Models.MindMap?> GetMindMapWithNodesAsync(int id)
    {
        return await _context.MindMaps
            .Include(m => m.FlashCardCollection)
            .Include(m => m.Nodes)
                .ThenInclude(n => n.FlashCard)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IEnumerable<Models.MindMap>> GetMindMapsByUserIdAsync(int userId)
    {
        return await _context.MindMaps
            .Include(m => m.FlashCardCollection)
            .Include(m => m.Nodes)
            .Where(m => m.UserId == userId)
            .OrderByDescending(m => m.UpdatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Models.MindMap>> GetMindMapsByCollectionIdAsync(int collectionId)
    {
        return await _context.MindMaps
            .Include(m => m.FlashCardCollection)
            .Include(m => m.Nodes)
            .Where(m => m.FlashCardCollectionId == collectionId)
            .OrderByDescending(m => m.UpdatedAt)
            .ToListAsync();
    }

    public async Task<Models.MindMap> CreateMindMapAsync(Models.MindMap mindMap)
    {
        _context.MindMaps.Add(mindMap);
        await _context.SaveChangesAsync();
        return mindMap;
    }

    public async Task<Models.MindMap> UpdateMindMapAsync(Models.MindMap mindMap)
    {
        _context.MindMaps.Update(mindMap);
        await _context.SaveChangesAsync();
        return mindMap;
    }

    public async Task<bool> DeleteMindMapAsync(int id)
    {
        var mindMap = await _context.MindMaps
            .Include(m => m.Nodes)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (mindMap == null)
            return false;

        // Remove all nodes first (cascade should handle this, but be explicit)
        _context.MindMapNodes.RemoveRange(mindMap.Nodes);
        _context.MindMaps.Remove(mindMap);
        await _context.SaveChangesAsync();
        return true;
    }

    // ──────────────── MindMapNode ────────────────

    public async Task<MindMapNode?> GetNodeByIdAsync(int id)
    {
        return await _context.MindMapNodes.FindAsync(id);
    }

    public async Task<MindMapNode?> GetNodeWithFlashCardAsync(int id)
    {
        return await _context.MindMapNodes
            .Include(n => n.FlashCard)
            .FirstOrDefaultAsync(n => n.Id == id);
    }

    public async Task<IEnumerable<MindMapNode>> GetNodesByMindMapIdAsync(int mindMapId)
    {
        return await _context.MindMapNodes
            .Include(n => n.FlashCard)
            .Where(n => n.MindMapId == mindMapId)
            .ToListAsync();
    }

    public async Task<MindMapNode> CreateNodeAsync(MindMapNode node)
    {
        _context.MindMapNodes.Add(node);
        await _context.SaveChangesAsync();

        // Reload with FlashCard included
        return await GetNodeWithFlashCardAsync(node.Id) ?? node;
    }

    public async Task<MindMapNode> UpdateNodeAsync(MindMapNode node)
    {
        _context.MindMapNodes.Update(node);
        await _context.SaveChangesAsync();

        // Reload with FlashCard included
        return await GetNodeWithFlashCardAsync(node.Id) ?? node;
    }

    public async Task<bool> DeleteNodeAsync(int id)
    {
        var node = await _context.MindMapNodes
            .Include(n => n.Children)
            .FirstOrDefaultAsync(n => n.Id == id);

        if (node == null)
            return false;

        // Recursively delete children
        await DeleteChildrenRecursive(node);
        _context.MindMapNodes.Remove(node);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task DeleteNodesByMindMapIdAsync(int mindMapId)
    {
        var nodes = await _context.MindMapNodes
            .Where(n => n.MindMapId == mindMapId)
            .ToListAsync();

        _context.MindMapNodes.RemoveRange(nodes);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<MindMapNode>> CreateNodesAsync(IEnumerable<MindMapNode> nodes)
    {
        await _context.MindMapNodes.AddRangeAsync(nodes);
        await _context.SaveChangesAsync();
        return nodes;
    }

    // ──────────────── Helpers ────────────────

    private async Task DeleteChildrenRecursive(MindMapNode node)
    {
        var children = await _context.MindMapNodes
            .Include(n => n.Children)
            .Where(n => n.ParentNodeId == node.Id)
            .ToListAsync();

        foreach (var child in children)
        {
            await DeleteChildrenRecursive(child);
            _context.MindMapNodes.Remove(child);
        }
    }
}
