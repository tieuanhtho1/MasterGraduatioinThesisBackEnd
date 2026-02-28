namespace WebAPI.Models.DTOs.MindMap;

public class UpdateMindMapNodeRequest
{
    public double PositionX { get; set; }
    public double PositionY { get; set; }
    public string Color { get; set; } = "#ffffff";
    public bool HideChildren { get; set; } = false;
    public int? ParentNodeId { get; set; }
}
