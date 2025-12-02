namespace WebAPI.Models.DTOs.FlashCardCollection;

public class FlashCardCollectionResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int? ParentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int FlashCardCount { get; set; }
    public int ChildrenCount { get; set; }
}
