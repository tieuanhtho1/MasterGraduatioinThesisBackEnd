namespace WebAPI.Models.DTOs.MindMap
{
    public class FullMindMapResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<MindMapNodeWithFlashCardDto> Nodes { get; set; } = new List<MindMapNodeWithFlashCardDto>();
    }
}
