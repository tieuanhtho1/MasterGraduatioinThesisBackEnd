namespace WebAPI.Models.DTOs.FlashCard;

public class UpdateFlashCardRequest
{
    public string Term { get; set; } = string.Empty;
    public string Definition { get; set; } = string.Empty;
    public int Score { get; set; }
}
