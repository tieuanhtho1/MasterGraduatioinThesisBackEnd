namespace WebAPI.Models.DTOs.MindMap
{
    public class CreateMindMapNodeDto
    {
        public int FlashCardId { get; set; }
        public int? ParentNodeId { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public string Color { get; set; } = "#3B82F6";
        public bool HideChildren { get; set; } = false;
    }
}
