using WebAPI.Models;

namespace WebAPI.Services.MindMap;

public interface IMindMapService
{
    // MindMap CRUD
    Task<Models.MindMap?> GetMindMapByIdAsync(int id);
    Task<Models.MindMap?> GetMindMapWithNodesAsync(int id);
    Task<IEnumerable<Models.MindMap>> GetMindMapsByUserIdAsync(int userId);
    Task<IEnumerable<Models.MindMap>> GetMindMapsByCollectionIdAsync(int collectionId);
    Task<Models.MindMap> CreateMindMapAsync(Models.MindMap mindMap);
    Task<Models.MindMap> UpdateMindMapAsync(Models.MindMap mindMap);
    Task<bool> DeleteMindMapAsync(int id);

    // MindMapNode CRUD
    Task<MindMapNode?> GetNodeByIdAsync(int id);
    Task<MindMapNode?> GetNodeWithFlashCardAsync(int id);
    Task<IEnumerable<MindMapNode>> GetNodesByMindMapIdAsync(int mindMapId);
    Task<MindMapNode> CreateNodeAsync(MindMapNode node);
    Task<MindMapNode> UpdateNodeAsync(MindMapNode node);
    Task<bool> DeleteNodeAsync(int id);
    Task DeleteNodesByMindMapIdAsync(int mindMapId);
    Task<IEnumerable<MindMapNode>> CreateNodesAsync(IEnumerable<MindMapNode> nodes);

    // MindMapEdge CRUD
    Task<IEnumerable<MindMapEdge>> GetEdgesByMindMapIdAsync(int mindMapId);
    Task<MindMapEdge?> GetEdgeByIdAsync(int id);
    Task<MindMapEdge> CreateEdgeAsync(MindMapEdge edge);
    Task<MindMapEdge> UpdateEdgeAsync(MindMapEdge edge);
    Task<bool> DeleteEdgeAsync(int id);
    Task DeleteEdgesByMindMapIdAsync(int mindMapId);
    Task<IEnumerable<MindMapEdge>> CreateEdgesAsync(IEnumerable<MindMapEdge> edges);
}
