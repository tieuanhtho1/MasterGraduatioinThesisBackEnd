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
            .Include(m => m.Edges)
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
            .Include(m => m.Edges)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (mindMap == null)
            return false;

        // Remove all edges first, then nodes (cascade should handle this, but be explicit)
        _context.MindMapEdges.RemoveRange(mindMap.Edges);
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
            .FirstOrDefaultAsync(n => n.Id == id);

        if (node == null)
            return false;

        // Delete all edges referencing this node
        var edges = await _context.MindMapEdges
            .Where(e => e.SourceNodeId == id || e.TargetNodeId == id)
            .ToListAsync();
        _context.MindMapEdges.RemoveRange(edges);

        _context.MindMapNodes.Remove(node);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task DeleteNodesByMindMapIdAsync(int mindMapId)
    {
        // Delete edges first
        var edges = await _context.MindMapEdges
            .Where(e => e.MindMapId == mindMapId)
            .ToListAsync();
        _context.MindMapEdges.RemoveRange(edges);

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

    // ──────────────── MindMapEdge ────────────────

    public async Task<IEnumerable<MindMapEdge>> GetEdgesByMindMapIdAsync(int mindMapId)
    {
        return await _context.MindMapEdges
            .Where(e => e.MindMapId == mindMapId)
            .ToListAsync();
    }

    public async Task<MindMapEdge?> GetEdgeByIdAsync(int id)
    {
        return await _context.MindMapEdges.FindAsync(id);
    }

    public async Task<MindMapEdge> CreateEdgeAsync(MindMapEdge edge)
    {
        _context.MindMapEdges.Add(edge);
        await _context.SaveChangesAsync();
        return edge;
    }

    public async Task<MindMapEdge> UpdateEdgeAsync(MindMapEdge edge)
    {
        _context.MindMapEdges.Update(edge);
        await _context.SaveChangesAsync();
        return edge;
    }

    public async Task<bool> DeleteEdgeAsync(int id)
    {
        var edge = await _context.MindMapEdges.FindAsync(id);
        if (edge == null)
            return false;

        _context.MindMapEdges.Remove(edge);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task DeleteEdgesByMindMapIdAsync(int mindMapId)
    {
        var edges = await _context.MindMapEdges
            .Where(e => e.MindMapId == mindMapId)
            .ToListAsync();

        _context.MindMapEdges.RemoveRange(edges);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<MindMapEdge>> CreateEdgesAsync(IEnumerable<MindMapEdge> edges)
    {
        await _context.MindMapEdges.AddRangeAsync(edges);
        await _context.SaveChangesAsync();
        return edges;
    }
}
