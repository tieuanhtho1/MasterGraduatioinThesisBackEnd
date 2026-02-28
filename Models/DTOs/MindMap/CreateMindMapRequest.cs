namespace WebAPI.Models.DTOs.MindMap;

public class CreateMindMapRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int UserId { get; set; }
    public int FlashCardCollectionId { get; set; }
}
