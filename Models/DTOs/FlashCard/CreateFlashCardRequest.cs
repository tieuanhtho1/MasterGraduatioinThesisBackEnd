namespace WebAPI.Models.DTOs.FlashCard;

public class CreateFlashCardRequest
{
    public string Term { get; set; } = string.Empty;
    public string Definition { get; set; } = string.Empty;
    public int Score { get; set; }
    public int FlashCardCollectionId { get; set; }
}
