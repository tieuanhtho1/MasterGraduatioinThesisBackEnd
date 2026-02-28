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
        existing.ParentNodeId = request.ParentNodeId;

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

    /// <summary>
    /// Bulk save all nodes for a mind map.
    /// Replaces all existing nodes with the provided set.
    /// This is the main save endpoint — frontend sends the entire node tree.
    /// </summary>
    public async Task<IEnumerable<MindMapNodeResponse>> SaveMindMapNodesAsync(int mindMapId, SaveMindMapNodesRequest request)
    {
        var mindMap = await _mindMapService.GetMindMapByIdAsync(mindMapId);
        if (mindMap == null)
            return Enumerable.Empty<MindMapNodeResponse>();

        // Delete all existing nodes for this mind map
        await _mindMapService.DeleteNodesByMindMapIdAsync(mindMapId);

        if (request.Nodes == null || !request.Nodes.Any())
        {
            mindMap.UpdatedAt = DateTime.UtcNow;
            await _mindMapService.UpdateMindMapAsync(mindMap);
            return Enumerable.Empty<MindMapNodeResponse>();
        }

        // Build a map from client-side temp IDs to track parent references
        // The frontend might send ParentNodeId referencing an Id from the previous save.
        // We need to handle the mapping of old IDs to new IDs.
        var oldIdToNewNode = new Dictionary<int, MindMapNode>();
        var nodesToCreate = new List<(MindMapNode node, int? oldParentId)>();

        foreach (var item in request.Nodes)
        {
            var node = new MindMapNode
            {
                PositionX = item.PositionX,
                PositionY = item.PositionY,
                Color = item.Color,
                HideChildren = item.HideChildren,
                MindMapId = mindMapId,
                FlashCardId = item.FlashCardId,
                ParentNodeId = null // Will be set after all nodes are created
            };

            nodesToCreate.Add((node, item.ParentNodeId));

            if (item.Id.HasValue && item.Id.Value > 0)
            {
                oldIdToNewNode[item.Id.Value] = node;
            }
        }

        // First pass: create all nodes without parent references
        var createdNodes = new List<MindMapNode>();
        foreach (var (node, _) in nodesToCreate)
        {
            var created = await _mindMapService.CreateNodeAsync(node);
            createdNodes.Add(created);
        }

        // Build mapping from old IDs to new IDs
        var oldToNewIdMap = new Dictionary<int, int>();
        for (int i = 0; i < nodesToCreate.Count; i++)
        {
            var item = request.Nodes[i];
            if (item.Id.HasValue && item.Id.Value > 0)
            {
                oldToNewIdMap[item.Id.Value] = createdNodes[i].Id;
            }
        }

        // Second pass: set parent references using the new IDs
        for (int i = 0; i < nodesToCreate.Count; i++)
        {
            var (_, oldParentId) = nodesToCreate[i];
            if (oldParentId.HasValue && oldToNewIdMap.ContainsKey(oldParentId.Value))
            {
                createdNodes[i].ParentNodeId = oldToNewIdMap[oldParentId.Value];
                await _mindMapService.UpdateNodeAsync(createdNodes[i]);
            }
        }

        // Update mind map timestamp
        mindMap.UpdatedAt = DateTime.UtcNow;
        await _mindMapService.UpdateMindMapAsync(mindMap);

        // Reload nodes with flash card info
        var finalNodes = await _mindMapService.GetNodesByMindMapIdAsync(mindMapId);
        return finalNodes.Select(MapNodeToResponse);
    }

    // ──────────────── Helpers ────────────────

    private static MindMapNodeResponse MapNodeToResponse(MindMapNode node)
    {
        return new MindMapNodeResponse
        {
            Id = node.Id,
            PositionX = node.PositionX,
            PositionY = node.PositionY,
            Color = node.Color,
            HideChildren = node.HideChildren,
            ParentNodeId = node.ParentNodeId,
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
}
