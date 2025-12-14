namespace WebAPI.Models.DTOs.LearnSession;

public class UpdateScoreRequest
{
    public List<ScoreUpdate> ScoreUpdates { get; set; } = new();
}

public class ScoreUpdate
{
    public int FlashCardId { get; set; }
    public int ScoreModification { get; set; }
}
