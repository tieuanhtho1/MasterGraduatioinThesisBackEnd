using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Models;
using WebAPI.Models.DTOs.MindMap;

namespace WebAPI.Services.Mindmap
{
    public class MindMapService : IMindMapService
    {
        private readonly ApplicationDbContext _context;

        public MindMapService(ApplicationDbContext context)
        {
            _context = context;
        }

        // MindMap operations
        public async Task<MindMap?> GetMindMapByIdAsync(int mindMapId, int userId)
        {
            return await _context.MindMaps
                .Include(m => m.Nodes)
                .FirstOrDefaultAsync(m => m.Id == mindMapId && m.UserId == userId);
        }

        public async Task<List<MindMap>> GetUserMindMapsAsync(int userId)
        {
            return await _context.MindMaps
                .Include(m => m.Nodes)
                .Where(m => m.UserId == userId)
                .OrderByDescending(m => m.UpdatedAt ?? m.CreatedAt)
                .ToListAsync();
        }

        public async Task<MindMap> CreateMindMapAsync(MindMap mindMap)
        {
            _context.MindMaps.Add(mindMap);
            await _context.SaveChangesAsync();
            return mindMap;
        }

        public async Task<MindMap?> UpdateMindMapAsync(int mindMapId, int userId, UpdateMindMapDto dto)
        {
            var mindMap = await GetMindMapByIdAsync(mindMapId, userId);
            if (mindMap == null) return null;

            if (!string.IsNullOrWhiteSpace(dto.Name))
                mindMap.Name = dto.Name;
            
            if (dto.Description != null)
                mindMap.Description = dto.Description;

            mindMap.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return mindMap;
        }

        public async Task<bool> DeleteMindMapAsync(int mindMapId, int userId)
        {
            var mindMap = await GetMindMapByIdAsync(mindMapId, userId);
            if (mindMap == null) return false;

            _context.MindMaps.Remove(mindMap);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<MindMap?> GetFullMindMapAsync(int mindMapId, int userId)
        {
            return await _context.MindMaps
                .Include(m => m.Nodes)
                    .ThenInclude(n => n.FlashCard)
                        .ThenInclude(f => f.FlashCardCollection)
                .FirstOrDefaultAsync(m => m.Id == mindMapId && m.UserId == userId);
        }

        // MindMapNode operations
        public async Task<MindMapNode?> GetNodeByIdAsync(int nodeId, int userId)
        {
            return await _context.MindMapNodes
                .Include(n => n.MindMap)
                .Include(n => n.FlashCard)
                .FirstOrDefaultAsync(n => n.Id == nodeId && n.MindMap.UserId == userId);
        }

        public async Task<MindMapNode> CreateNodeAsync(MindMapNode node)
        {
            _context.MindMapNodes.Add(node);
            await _context.SaveChangesAsync();
            return node;
        }

        public async Task<MindMapNode?> UpdateNodeAsync(int nodeId, int userId, UpdateMindMapNodeDto dto)
        {
            var node = await GetNodeByIdAsync(nodeId, userId);
            if (node == null) return null;

            if (dto.ParentNodeId.HasValue)
            {
                // Validate parent node exists and belongs to same mindmap
                if (dto.ParentNodeId.Value != 0)
                {
                    var parentNode = await _context.MindMapNodes
                        .FirstOrDefaultAsync(n => n.Id == dto.ParentNodeId.Value && n.MindMapId == node.MindMapId);
                    if (parentNode == null)
                        throw new InvalidOperationException("Parent node not found or doesn't belong to the same mindmap");
                }
                node.ParentNodeId = dto.ParentNodeId.Value == 0 ? null : dto.ParentNodeId.Value;
            }

            if (dto.PositionX.HasValue)
                node.PositionX = dto.PositionX.Value;

            if (dto.PositionY.HasValue)
                node.PositionY = dto.PositionY.Value;

            if (!string.IsNullOrWhiteSpace(dto.Color))
                node.Color = dto.Color;

            if (dto.HideChildren.HasValue)
                node.HideChildren = dto.HideChildren.Value;

            node.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return node;
        }

        public async Task<bool> DeleteNodeAsync(int nodeId, int userId)
        {
            var node = await GetNodeByIdAsync(nodeId, userId);
            if (node == null) return false;

            // Update children to remove parent reference (make them root nodes)
            var childNodes = await _context.MindMapNodes
                .Where(n => n.ParentNodeId == nodeId)
                .ToListAsync();

            foreach (var child in childNodes)
            {
                child.ParentNodeId = null;
            }

            _context.MindMapNodes.Remove(node);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<MindMapNode>> GetNodesByMindMapIdAsync(int mindMapId)
        {
            return await _context.MindMapNodes
                .Include(n => n.FlashCard)
                    .ThenInclude(f => f.FlashCardCollection)
                .Where(n => n.MindMapId == mindMapId)
                .ToListAsync();
        }

        public async Task<bool> ValidateFlashCardAccessAsync(int flashCardId, int userId)
        {
            return await _context.FlashCards
                .AnyAsync(f => f.Id == flashCardId && f.FlashCardCollection.UserId == userId);
        }
    }
}
