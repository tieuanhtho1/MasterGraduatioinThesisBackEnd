namespace WebAPI.Models.DTOs.FlashCard;

public class BulkCreateFlashCardsRequest
{
    public int FlashCardCollectionId { get; set; }
    public List<FlashCardItem> FlashCards { get; set; } = new();
}

public class FlashCardItem
{
    public int? Id { get; set; }
    public string Term { get; set; } = string.Empty;
    public string Definition { get; set; } = string.Empty;
    public int Score { get; set; }
}
