namespace WebAPI.Models.DTOs.FlashCardCollection;

public class CreateFlashCardCollectionRequest
{
    public int UserId { get; set; }
    public int? ParentId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
