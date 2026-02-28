namespace WebAPI.Models.DTOs.MindMap;

public class CreateMindMapNodeRequest
{
    public double PositionX { get; set; }
    public double PositionY { get; set; }
    public string Color { get; set; } = "#ffffff";
    public bool HideChildren { get; set; } = false;
    public int? ParentNodeId { get; set; }
    public int MindMapId { get; set; }
    public int FlashCardId { get; set; }
}
