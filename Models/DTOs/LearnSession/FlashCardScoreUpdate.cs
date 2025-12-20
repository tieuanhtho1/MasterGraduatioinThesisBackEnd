namespace WebAPI.Models.DTOs.LearnSession;

public class FlashCardScoreUpdate
{
    public int FlashCardId { get; set; }
    public int ScoreModification { get; set; }
    public int TimesLearned { get; set; }
}
