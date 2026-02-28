using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.BusinessLogic.MindMap;
using WebAPI.Models;
using WebAPI.Models.DTOs.MindMap;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MindMapController : ControllerBase
{
    private readonly IMindMapBusinessLogic _mindMapBusinessLogic;

    public MindMapController(IMindMapBusinessLogic mindMapBusinessLogic)
    {
        _mindMapBusinessLogic = mindMapBusinessLogic;
    }

    // ══════════════════════════════════════════
    //  MIND MAP ENDPOINTS
    // ══════════════════════════════════════════

    /// <summary>
    /// Get a mind map by ID (metadata only, no nodes)
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMindMap(int id)
    {
        var mindMap = await _mindMapBusinessLogic.GetMindMapByIdAsync(id);
        if (mindMap == null)
            return NotFound(new { message = "Mind map not found" });

        return Ok(MapToResponse(mindMap));
    }

    /// <summary>
    /// Get a mind map with all nodes, edges, and their flash card data.
    /// This is the main endpoint for React Flow rendering.
    /// </summary>
    [HttpGet("{id}/detail")]
    public async Task<IActionResult> GetMindMapDetail(int id)
    {
        var mindMap = await _mindMapBusinessLogic.GetMindMapWithNodesAsync(id);
        if (mindMap == null)
            return NotFound(new { message = "Mind map not found" });

        var response = new MindMapDetailResponse
        {
            Id = mindMap.Id,
            Title = mindMap.Title,
            Description = mindMap.Description,
            UserId = mindMap.UserId,
            FlashCardCollectionId = mindMap.FlashCardCollectionId,
            CollectionTitle = mindMap.FlashCardCollection?.Title ?? string.Empty,
            CreatedAt = mindMap.CreatedAt,
            UpdatedAt = mindMap.UpdatedAt,
            Nodes = mindMap.Nodes?.Select(n => new MindMapNodeResponse
            {
                Id = n.Id,
                PositionX = n.PositionX,
                PositionY = n.PositionY,
                Color = n.Color,
                HideChildren = n.HideChildren,
                MindMapId = n.MindMapId,
                FlashCardId = n.FlashCardId,
                FlashCard = n.FlashCard != null ? new FlashCardInfo
                {
                    Id = n.FlashCard.Id,
                    Term = n.FlashCard.Term,
                    Definition = n.FlashCard.Definition,
                    Score = n.FlashCard.Score,
                    TimesLearned = n.FlashCard.TimesLearned,
                    FlashCardCollectionId = n.FlashCard.FlashCardCollectionId
                } : null!
            }).ToList() ?? new(),
            Edges = mindMap.Edges?.Select(e => new MindMapEdgeResponse
            {
                Id = e.Id,
                SourceNodeId = e.SourceNodeId,
                TargetNodeId = e.TargetNodeId,
                SourceHandle = e.SourceHandle,
                TargetHandle = e.TargetHandle,
                MindMapId = e.MindMapId
            }).ToList() ?? new()
        };

        return Ok(response);
    }

    /// <summary>
    /// Get all mind maps for a user
    /// </summary>
    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetMindMapsByUser(int userId)
    {
        var mindMaps = await _mindMapBusinessLogic.GetMindMapsByUserIdAsync(userId);
        var response = mindMaps.Select(MapToResponse);
        return Ok(response);
    }

    /// <summary>
    /// Get all mind maps for a collection
    /// </summary>
    [HttpGet("collection/{collectionId}")]
    public async Task<IActionResult> GetMindMapsByCollection(int collectionId)
    {
        var mindMaps = await _mindMapBusinessLogic.GetMindMapsByCollectionIdAsync(collectionId);
        var response = mindMaps.Select(MapToResponse);
        return Ok(response);
    }

    /// <summary>
    /// Create a new mind map
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateMindMap([FromBody] CreateMindMapRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return BadRequest(new { message = "Title is required" });

        if (request.UserId <= 0)
            return BadRequest(new { message = "Valid UserId is required" });

        if (request.FlashCardCollectionId <= 0)
            return BadRequest(new { message = "Valid FlashCardCollectionId is required" });

        var mindMap = new Models.MindMap
        {
            Title = request.Title,
            Description = request.Description,
            UserId = request.UserId,
            FlashCardCollectionId = request.FlashCardCollectionId
        };

        var result = await _mindMapBusinessLogic.CreateMindMapAsync(mindMap);
        if (result == null)
            return BadRequest(new { message = "Failed to create mind map" });

        // Reload to get collection info
        var loaded = await _mindMapBusinessLogic.GetMindMapByIdAsync(result.Id);
        return CreatedAtAction(nameof(GetMindMap), new { id = result.Id }, MapToResponse(loaded!));
    }

    /// <summary>
    /// Update a mind map (title, description, collection)
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateMindMap(int id, [FromBody] UpdateMindMapRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Title))
            return BadRequest(new { message = "Title is required" });

        var mindMap = new Models.MindMap
        {
            Title = request.Title,
            Description = request.Description,
            FlashCardCollectionId = request.FlashCardCollectionId ?? 0
        };

        var result = await _mindMapBusinessLogic.UpdateMindMapAsync(id, mindMap);
        if (result == null)
            return NotFound(new { message = "Mind map not found" });

        // Reload to get collection info
        var loaded = await _mindMapBusinessLogic.GetMindMapByIdAsync(result.Id);
        return Ok(MapToResponse(loaded!));
    }

    /// <summary>
    /// Delete a mind map and all its nodes and edges
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMindMap(int id)
    {
        var result = await _mindMapBusinessLogic.DeleteMindMapAsync(id);
        if (!result)
            return NotFound(new { message = "Mind map not found" });

        return Ok(new { message = "Mind map deleted successfully" });
    }

    // ══════════════════════════════════════════
    //  MIND MAP NODE ENDPOINTS
    // ══════════════════════════════════════════

    /// <summary>
    /// Get a single node by ID (with flash card info)
    /// </summary>
    [HttpGet("node/{nodeId}")]
    public async Task<IActionResult> GetNode(int nodeId)
    {
        var node = await _mindMapBusinessLogic.GetNodeByIdAsync(nodeId);
        if (node == null)
            return NotFound(new { message = "Node not found" });

        var response = new MindMapNodeResponse
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

        return Ok(response);
    }

    /// <summary>
    /// Add a single node to a mind map
    /// </summary>
    [HttpPost("node")]
    public async Task<IActionResult> CreateNode([FromBody] CreateMindMapNodeRequest request)
    {
        if (request.MindMapId <= 0)
            return BadRequest(new { message = "Valid MindMapId is required" });

        if (request.FlashCardId <= 0)
            return BadRequest(new { message = "Valid FlashCardId is required" });

        var node = new MindMapNode
        {
            PositionX = request.PositionX,
            PositionY = request.PositionY,
            Color = request.Color,
            HideChildren = request.HideChildren,
            MindMapId = request.MindMapId,
            FlashCardId = request.FlashCardId
        };

        var result = await _mindMapBusinessLogic.CreateNodeAsync(node);
        if (result == null)
            return BadRequest(new { message = "Failed to create node" });

        return CreatedAtAction(nameof(GetNode), new { nodeId = result.Id }, result);
    }

    /// <summary>
    /// Update a single node (position, color, hideChildren)
    /// </summary>
    [HttpPut("node/{nodeId}")]
    public async Task<IActionResult> UpdateNode(int nodeId, [FromBody] UpdateMindMapNodeRequest request)
    {
        var result = await _mindMapBusinessLogic.UpdateNodeAsync(nodeId, request);
        if (result == null)
            return NotFound(new { message = "Node not found" });

        return Ok(result);
    }

    /// <summary>
    /// Delete a node. All edges referencing this node are cascade deleted.
    /// </summary>
    [HttpDelete("node/{nodeId}")]
    public async Task<IActionResult> DeleteNode(int nodeId)
    {
        var result = await _mindMapBusinessLogic.DeleteNodeAsync(nodeId);
        if (!result)
            return NotFound(new { message = "Node not found" });

        return Ok(new { message = "Node deleted successfully" });
    }

    // ══════════════════════════════════════════
    //  MIND MAP EDGE ENDPOINTS
    // ══════════════════════════════════════════

    /// <summary>
    /// Get all edges for a mind map
    /// </summary>
    [HttpGet("{mindMapId}/edges")]
    public async Task<IActionResult> GetEdges(int mindMapId)
    {
        var edges = await _mindMapBusinessLogic.GetEdgesByMindMapIdAsync(mindMapId);
        return Ok(edges);
    }

    /// <summary>
    /// Delete a single edge by its ID
    /// </summary>
    [HttpDelete("edge/{edgeId}")]
    public async Task<IActionResult> DeleteEdge(int edgeId)
    {
        var result = await _mindMapBusinessLogic.DeleteEdgeAsync(edgeId);
        if (!result)
            return NotFound(new { message = "Edge not found" });

        return NoContent();
    }

    // ══════════════════════════════════════════
    //  BULK SAVE (NODES + EDGES)
    // ══════════════════════════════════════════

    /// <summary>
    /// Save (bulk) all nodes and edges of a mind map.
    /// Frontend sends the entire node tree and edge list; backend syncs accordingly.
    /// </summary>
    [HttpPut("{mindMapId}/nodes")]
    public async Task<IActionResult> SaveMindMapNodes(int mindMapId, [FromBody] SaveMindMapNodesRequest request)
    {
        var result = await _mindMapBusinessLogic.SaveMindMapNodesAsync(mindMapId, request);
        if (result == null)
            return NotFound(new { message = "Mind map not found" });

        return Ok(new
        {
            message = "Saved successfully",
            nodes = result.Nodes,
            edges = result.Edges
        });
    }

    // ──────────────── Helpers ────────────────

    private static MindMapResponse MapToResponse(Models.MindMap mindMap)
    {
        return new MindMapResponse
        {
            Id = mindMap.Id,
            Title = mindMap.Title,
            Description = mindMap.Description,
            UserId = mindMap.UserId,
            FlashCardCollectionId = mindMap.FlashCardCollectionId,
            CollectionTitle = mindMap.FlashCardCollection?.Title ?? string.Empty,
            NodeCount = mindMap.Nodes?.Count ?? 0,
            CreatedAt = mindMap.CreatedAt,
            UpdatedAt = mindMap.UpdatedAt
        };
    }
}
