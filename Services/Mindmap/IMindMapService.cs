using WebAPI.Models;
using WebAPI.Models.DTOs.MindMap;

namespace WebAPI.Services.Mindmap
{
    public interface IMindMapService
    {
        // MindMap operations
        Task<MindMap?> GetMindMapByIdAsync(int mindMapId, int userId);
        Task<List<MindMap>> GetUserMindMapsAsync(int userId);
        Task<MindMap> CreateMindMapAsync(MindMap mindMap);
        Task<MindMap?> UpdateMindMapAsync(int mindMapId, int userId, UpdateMindMapDto dto);
        Task<bool> DeleteMindMapAsync(int mindMapId, int userId);
        Task<MindMap?> GetFullMindMapAsync(int mindMapId, int userId);

        // MindMapNode operations
        Task<MindMapNode?> GetNodeByIdAsync(int nodeId, int userId);
        Task<MindMapNode> CreateNodeAsync(MindMapNode node);
        Task<MindMapNode?> UpdateNodeAsync(int nodeId, int userId, UpdateMindMapNodeDto dto);
        Task<bool> DeleteNodeAsync(int nodeId, int userId);
        Task<List<MindMapNode>> GetNodesByMindMapIdAsync(int mindMapId);
        Task<bool> ValidateFlashCardAccessAsync(int flashCardId, int userId);
    }
}
