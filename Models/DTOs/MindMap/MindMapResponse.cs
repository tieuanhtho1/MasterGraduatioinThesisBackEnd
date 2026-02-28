namespace WebAPI.Models.DTOs.MindMap;

public class MindMapResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int UserId { get; set; }
    public int FlashCardCollectionId { get; set; }
    public string CollectionTitle { get; set; } = string.Empty;
    public int NodeCount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
