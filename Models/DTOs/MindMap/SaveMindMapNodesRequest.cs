namespace WebAPI.Models.DTOs.MindMap;

/// <summary>
/// Used for the bulk save operation — saves all nodes of a mind map at once.
/// This is the main save endpoint the frontend calls when saving the entire mind map.
/// </summary>
public class SaveMindMapNodesRequest
{
    public List<SaveMindMapNodeItem> Nodes { get; set; } = new();
    public List<BulkSaveEdgeDto> Edges { get; set; } = new();
}

public class SaveMindMapNodeItem
{
    /// <summary>
    /// If Id is provided and > 0, update existing node. Otherwise, create new.
    /// </summary>
    public int? Id { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
    public string Color { get; set; } = "#ffffff";
    public bool HideChildren { get; set; } = false;
    public int FlashCardId { get; set; }
}
