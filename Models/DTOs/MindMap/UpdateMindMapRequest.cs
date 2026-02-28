namespace WebAPI.Models.DTOs.MindMap;

public class UpdateMindMapRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? FlashCardCollectionId { get; set; }
}
