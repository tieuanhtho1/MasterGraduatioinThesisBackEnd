namespace WebAPI.Models.DTOs.MindMap;

/// <summary>
/// Full mind map detail including all nodes with flash card info.
/// Used for the React Flow rendering on the frontend.
/// </summary>
public class MindMapDetailResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int UserId { get; set; }
    public int FlashCardCollectionId { get; set; }
    public string CollectionTitle { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<MindMapNodeResponse> Nodes { get; set; } = new();
}
