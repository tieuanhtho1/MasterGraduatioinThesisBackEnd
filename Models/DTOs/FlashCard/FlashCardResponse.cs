namespace WebAPI.Models.DTOs.FlashCard;

public class FlashCardResponse
{
    public int Id { get; set; }
    public string Term { get; set; } = string.Empty;
    public string Definition { get; set; } = string.Empty;
    public int Score { get; set; }
    public int FlashCardCollectionId { get; set; }
}
