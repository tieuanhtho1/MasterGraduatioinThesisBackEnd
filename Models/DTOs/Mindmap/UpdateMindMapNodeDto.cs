namespace WebAPI.Models.DTOs.MindMap
{
    public class UpdateMindMapNodeDto
    {
        public int? ParentNodeId { get; set; }
        public double? PositionX { get; set; }
        public double? PositionY { get; set; }
        public string? Color { get; set; }
        public bool? HideChildren { get; set; }
    }
}
