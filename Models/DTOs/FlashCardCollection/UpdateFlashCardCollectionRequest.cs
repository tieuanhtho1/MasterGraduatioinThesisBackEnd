namespace WebAPI.Models.DTOs.FlashCardCollection;

public class UpdateFlashCardCollectionRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? ParentId { get; set; }
}
