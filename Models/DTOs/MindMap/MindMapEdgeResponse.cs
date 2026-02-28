namespace WebAPI.Models.DTOs.MindMap;

public class MindMapEdgeResponse
{
    public int Id { get; set; }
    public int SourceNodeId { get; set; }
    public int TargetNodeId { get; set; }
    public string SourceHandle { get; set; } = string.Empty;
    public string TargetHandle { get; set; } = string.Empty;
    public int MindMapId { get; set; }
}

public class BulkSaveEdgeDto
{
    /// <summary>
    /// Existing edge ID, or null for new edges.
    /// </summary>
    public int? Id { get; set; }
    public int SourceNodeId { get; set; }
    public int TargetNodeId { get; set; }
    public string SourceHandle { get; set; } = string.Empty;
    public string TargetHandle { get; set; } = string.Empty;
}
