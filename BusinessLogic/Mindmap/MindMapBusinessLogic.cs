using WebAPI.Models;
using WebAPI.Models.DTOs.MindMap;
using WebAPI.Services.MindMap;

namespace WebAPI.BusinessLogic.MindMap;

public class MindMapBusinessLogic : IMindMapBusinessLogic
{
    private readonly IMindMapService _mindMapService;

    public MindMapBusinessLogic(IMindMapService mindMapService)
    {
        _mindMapService = mindMapService;
    }

    // ──────────────── MindMap ────────────────

    public async Task<Models.MindMap?> GetMindMapByIdAsync(int id)
    {
        return await _mindMapService.GetMindMapByIdAsync(id);
    }

    public async Task<Models.MindMap?> GetMindMapWithNodesAsync(int id)
    {
        return await _mindMapService.GetMindMapWithNodesAsync(id);
    }

    public async Task<IEnumerable<Models.MindMap>> GetMindMapsByUserIdAsync(int userId)
    {
        return await _mindMapService.GetMindMapsByUserIdAsync(userId);
    }

    public async Task<IEnumerable<Models.MindMap>> GetMindMapsByCollectionIdAsync(int collectionId)
    {
        return await _mindMapService.GetMindMapsByCollectionIdAsync(collectionId);
    }

    public async Task<Models.MindMap?> CreateMindMapAsync(Models.MindMap mindMap)
    {
        if (string.IsNullOrWhiteSpace(mindMap.Title))
            return null;

        mindMap.CreatedAt = DateTime.UtcNow;
        mindMap.UpdatedAt = DateTime.UtcNow;

        return await _mindMapService.CreateMindMapAsync(mindMap);
    }

    public async Task<Models.MindMap?> UpdateMindMapAsync(int id, Models.MindMap mindMap)
    {
        var existing = await _mindMapService.GetMindMapByIdAsync(id);
        if (existing == null)
            return null;

        existing.Title = mindMap.Title;
        existing.Description = mindMap.Description;
        existing.UpdatedAt = DateTime.UtcNow;

        if (mindMap.FlashCardCollectionId > 0)
            existing.FlashCardCollectionId = mindMap.FlashCardCollectionId;

        return await _mindMapService.UpdateMindMapAsync(existing);
    }

    public async Task<bool> DeleteMindMapAsync(int id)
    {
        var mindMap = await _mindMapService.GetMindMapByIdAsync(id);
        if (mindMap == null)
            return false;

        return await _mindMapService.DeleteMindMapAsync(id);
    }

    // ──────────────── MindMapNode ────────────────

    public async Task<MindMapNode?> GetNodeByIdAsync(int id)
    {
        return await _mindMapService.GetNodeWithFlashCardAsync(id);
    }

    public async Task<MindMapNodeResponse?> CreateNodeAsync(MindMapNode node)
    {
        if (node.FlashCardId <= 0 || node.MindMapId <= 0)
            return null;

        var created = await _mindMapService.CreateNodeAsync(node);

        // Update mind map timestamp
        var mindMap = await _mindMapService.GetMindMapByIdAsync(node.MindMapId);
        if (mindMap != null)
        {
            mindMap.UpdatedAt = DateTime.UtcNow;
            await _mindMapService.UpdateMindMapAsync(mindMap);
        }

        return MapNodeToResponse(created);
    }

    public async Task<MindMapNodeResponse?> UpdateNodeAsync(int id, UpdateMindMapNodeRequest request)
    {
        var existing = await _mindMapService.GetNodeByIdAsync(id);
        if (existing == null)
            return null;

        existing.PositionX = request.PositionX;
        existing.PositionY = request.PositionY;
        existing.Color = request.Color;
        existing.HideChildren = request.HideChildren;

        var updated = await _mindMapService.UpdateNodeAsync(existing);

        // Update mind map timestamp
        var mindMap = await _mindMapService.GetMindMapByIdAsync(existing.MindMapId);
        if (mindMap != null)
        {
            mindMap.UpdatedAt = DateTime.UtcNow;
            await _mindMapService.UpdateMindMapAsync(mindMap);
        }

        return MapNodeToResponse(updated);
    }

    public async Task<bool> DeleteNodeAsync(int id)
    {
        var node = await _mindMapService.GetNodeByIdAsync(id);
        if (node == null)
            return false;

        var mindMapId = node.MindMapId;
        var result = await _mindMapService.DeleteNodeAsync(id);

        if (result)
        {
            // Update mind map timestamp
            var mindMap = await _mindMapService.GetMindMapByIdAsync(mindMapId);
            if (mindMap != null)
            {
                mindMap.UpdatedAt = DateTime.UtcNow;
                await _mindMapService.UpdateMindMapAsync(mindMap);
            }
        }

        return result;
    }

    // ──────────────── MindMapEdge ────────────────

    public async Task<IEnumerable<MindMapEdgeResponse>> GetEdgesByMindMapIdAsync(int mindMapId)
    {
        var edges = await _mindMapService.GetEdgesByMindMapIdAsync(mindMapId);
        return edges.Select(MapEdgeToResponse);
    }

    public async Task<bool> DeleteEdgeAsync(int id)
    {
        var edge = await _mindMapService.GetEdgeByIdAsync(id);
        if (edge == null)
            return false;

        var mindMapId = edge.MindMapId;
        var result = await _mindMapService.DeleteEdgeAsync(id);

        if (result)
        {
            var mindMap = await _mindMapService.GetMindMapByIdAsync(mindMapId);
            if (mindMap != null)
            {
                mindMap.UpdatedAt = DateTime.UtcNow;
                await _mindMapService.UpdateMindMapAsync(mindMap);
            }
        }

        return result;
    }

    // ──────────────── Bulk Save ────────────────

    /// <summary>
    /// Bulk save all nodes and edges for a mind map.
    /// Handles create/update/delete for both nodes and edges.
    /// Resolves temp negative IDs for new nodes referenced in edges.
    /// </summary>
    public async Task<SaveMindMapNodesResult?> SaveMindMapNodesAsync(int mindMapId, SaveMindMapNodesRequest request)
    {
        var mindMap = await _mindMapService.GetMindMapByIdAsync(mindMapId);
        if (mindMap == null)
            return null;

        // ── Step 1: Process Nodes ──

        // Get existing nodes for this mind map
        var existingNodes = (await _mindMapService.GetNodesByMindMapIdAsync(mindMapId)).ToList();
        var existingNodeIds = existingNodes.Select(n => n.Id).ToHashSet();

        // Determine which node IDs are in the request
        var requestNodeIds = request.Nodes
            .Where(n => n.Id.HasValue && n.Id.Value > 0)
            .Select(n => n.Id!.Value)
            .ToHashSet();

        // Delete nodes not in the request (they were removed by the user)
        // First delete edges for those nodes, then the nodes themselves
        var nodeIdsToDelete = existingNodeIds.Except(requestNodeIds).ToList();
        if (nodeIdsToDelete.Any())
        {
            foreach (var nodeId in nodeIdsToDelete)
            {
                await _mindMapService.DeleteNodeAsync(nodeId);
            }
        }

        // Map from temp/old IDs to real DB IDs
        var idMapping = new Dictionary<int, int>();

        foreach (var item in request.Nodes)
        {
            if (item.Id.HasValue && item.Id.Value > 0 && existingNodeIds.Contains(item.Id.Value))
            {
                // Update existing node
                var existing = existingNodes.First(n => n.Id == item.Id.Value);
                existing.PositionX = item.PositionX;
                existing.PositionY = item.PositionY;
                existing.Color = item.Color;
                existing.HideChildren = item.HideChildren;
                existing.FlashCardId = item.FlashCardId;
                await _mindMapService.UpdateNodeAsync(existing);
                idMapping[item.Id.Value] = existing.Id;
            }
            else
            {
                // Create new node
                var tempId = item.Id ?? 0;
                var node = new MindMapNode
                {
                    PositionX = item.PositionX,
                    PositionY = item.PositionY,
                    Color = item.Color,
                    HideChildren = item.HideChildren,
                    MindMapId = mindMapId,
                    FlashCardId = item.FlashCardId
                };
                var created = await _mindMapService.CreateNodeAsync(node);
                if (tempId != 0)
                {
                    idMapping[tempId] = created.Id;
                }
            }
        }

        // ── Step 2: Process Edges ──

        // Get existing edges for this mind map
        var existingEdges = (await _mindMapService.GetEdgesByMindMapIdAsync(mindMapId)).ToList();
        var existingEdgeIds = existingEdges.Select(e => e.Id).ToHashSet();

        // Determine which edge IDs are in the request
        var requestEdgeIds = request.Edges
            .Where(e => e.Id.HasValue && e.Id.Value > 0)
            .Select(e => e.Id!.Value)
            .ToHashSet();

        // Delete edges not in the request
        var edgeIdsToDelete = existingEdgeIds.Except(requestEdgeIds).ToList();
        foreach (var edgeId in edgeIdsToDelete)
        {
            await _mindMapService.DeleteEdgeAsync(edgeId);
        }

        // Update existing edges and create new ones
        foreach (var edgeDto in request.Edges)
        {
            // Resolve node IDs (handle temp negative IDs for new nodes)
            var sourceNodeId = ResolveNodeId(edgeDto.SourceNodeId, idMapping);
            var targetNodeId = ResolveNodeId(edgeDto.TargetNodeId, idMapping);

            if (edgeDto.Id.HasValue && edgeDto.Id.Value > 0 && existingEdgeIds.Contains(edgeDto.Id.Value))
            {
                // Update existing edge
                var existing = existingEdges.First(e => e.Id == edgeDto.Id.Value);
                existing.SourceNodeId = sourceNodeId;
                existing.TargetNodeId = targetNodeId;
                existing.SourceHandle = edgeDto.SourceHandle;
                existing.TargetHandle = edgeDto.TargetHandle;
                await _mindMapService.UpdateEdgeAsync(existing);
            }
            else
            {
                // Create new edge
                var edge = new MindMapEdge
                {
                    SourceNodeId = sourceNodeId,
                    TargetNodeId = targetNodeId,
                    SourceHandle = edgeDto.SourceHandle,
                    TargetHandle = edgeDto.TargetHandle,
                    MindMapId = mindMapId
                };
                await _mindMapService.CreateEdgeAsync(edge);
            }
        }

        // Update mind map timestamp
        mindMap.UpdatedAt = DateTime.UtcNow;
        await _mindMapService.UpdateMindMapAsync(mindMap);

        // Reload nodes and edges with full data
        var finalNodes = await _mindMapService.GetNodesByMindMapIdAsync(mindMapId);
        var finalEdges = await _mindMapService.GetEdgesByMindMapIdAsync(mindMapId);

        return new SaveMindMapNodesResult
        {
            Nodes = finalNodes.Select(MapNodeToResponse).ToList(),
            Edges = finalEdges.Select(MapEdgeToResponse).ToList()
        };
    }

    // ──────────────── Helpers ────────────────

    private static int ResolveNodeId(int nodeId, Dictionary<int, int> idMapping)
    {
        // If the nodeId is in the mapping (either a temp negative ID or an old positive ID), resolve it
        if (idMapping.TryGetValue(nodeId, out var resolvedId))
            return resolvedId;

        // Otherwise, use the nodeId as-is (it's already a valid DB ID)
        return nodeId;
    }

    private static MindMapNodeResponse MapNodeToResponse(MindMapNode node)
    {
        return new MindMapNodeResponse
        {
            Id = node.Id,
            PositionX = node.PositionX,
            PositionY = node.PositionY,
            Color = node.Color,
            HideChildren = node.HideChildren,
            MindMapId = node.MindMapId,
            FlashCardId = node.FlashCardId,
            FlashCard = node.FlashCard != null ? new FlashCardInfo
            {
                Id = node.FlashCard.Id,
                Term = node.FlashCard.Term,
                Definition = node.FlashCard.Definition,
                Score = node.FlashCard.Score,
                TimesLearned = node.FlashCard.TimesLearned,
                FlashCardCollectionId = node.FlashCard.FlashCardCollectionId
            } : null!
        };
    }

    private static MindMapEdgeResponse MapEdgeToResponse(MindMapEdge edge)
    {
        return new MindMapEdgeResponse
        {
            Id = edge.Id,
            SourceNodeId = edge.SourceNodeId,
            TargetNodeId = edge.TargetNodeId,
            SourceHandle = edge.SourceHandle,
            TargetHandle = edge.TargetHandle,
            MindMapId = edge.MindMapId
        };
    }
}
