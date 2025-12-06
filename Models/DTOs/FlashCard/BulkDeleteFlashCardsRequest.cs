namespace WebAPI.Models.DTOs.FlashCard;

public class BulkDeleteFlashCardsRequest
{
    public List<int> FlashCardIds { get; set; } = new();
}
