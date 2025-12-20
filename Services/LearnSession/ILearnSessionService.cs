using WebAPI.Models.DTOs.LearnSession;

namespace WebAPI.Services.LearnSession;

public interface ILearnSessionService
{
    Task<List<Models.FlashCard>> GetFlashCardsFromCollectionTreeAsync(int collectionId);
    Task<bool> UpdateFlashCardScoresAsync(List<FlashCardScoreUpdate> scoreUpdates);
}
