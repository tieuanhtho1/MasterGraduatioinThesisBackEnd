using WebAPI.Models;
using WebAPI.Models.DTOs.MindMap;

namespace WebAPI.BusinessLogic.MindMap;

public interface IMindMapBusinessLogic
{
    // MindMap operations
    Task<Models.MindMap?> GetMindMapByIdAsync(int id);
    Task<Models.MindMap?> GetMindMapWithNodesAsync(int id);
    Task<IEnumerable<Models.MindMap>> GetMindMapsByUserIdAsync(int userId);
    Task<IEnumerable<Models.MindMap>> GetMindMapsByCollectionIdAsync(int collectionId);
    Task<Models.MindMap?> CreateMindMapAsync(Models.MindMap mindMap);
    Task<Models.MindMap?> UpdateMindMapAsync(int id, Models.MindMap mindMap);
    Task<bool> DeleteMindMapAsync(int id);

    // Node operations
    Task<MindMapNode?> GetNodeByIdAsync(int id);
    Task<MindMapNodeResponse?> CreateNodeAsync(MindMapNode node);
    Task<MindMapNodeResponse?> UpdateNodeAsync(int id, UpdateMindMapNodeRequest request);
    Task<bool> DeleteNodeAsync(int id);
    Task<IEnumerable<MindMapNodeResponse>> SaveMindMapNodesAsync(int mindMapId, SaveMindMapNodesRequest request);
}
