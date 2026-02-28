namespace WebAPI.Models.DTOs.MindMap;

/// <summary>
/// Result of a bulk save operation for mind map nodes and edges.
/// </summary>
public class SaveMindMapNodesResult
{
    public List<MindMapNodeResponse> Nodes { get; set; } = new();
    public List<MindMapEdgeResponse> Edges { get; set; } = new();
}
