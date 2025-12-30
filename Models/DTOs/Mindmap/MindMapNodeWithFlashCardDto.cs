namespace WebAPI.Models.DTOs.MindMap
{
    public class MindMapNodeWithFlashCardDto
    {
        public int Id { get; set; }
        public int MindMapId { get; set; }
        public int? ParentNodeId { get; set; }
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public string Color { get; set; } = string.Empty;
        public bool HideChildren { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        // FlashCard information
        public FlashCardInfo FlashCard { get; set; } = null!;
    }

    public class FlashCardInfo
    {
        public int Id { get; set; }
        public string Term { get; set; } = string.Empty;
        public string Definition { get; set; } = string.Empty;
        public int Score { get; set; }
        public int LearnCount { get; set; }
        public int CollectionId { get; set; }
        public string CollectionName { get; set; } = string.Empty;
    }
}
