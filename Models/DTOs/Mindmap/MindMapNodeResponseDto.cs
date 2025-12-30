namespace WebAPI.Models.DTOs.MindMap
{
    public class MindMapNodeResponseDto
    {
        public int Id { get; set; }
        public int MindMapId { get; set; }
        public int FlashCardId { get; set; }
        public int? ParentNodeId { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public string Color { get; set; } = string.Empty;
        public bool HideChildren { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
