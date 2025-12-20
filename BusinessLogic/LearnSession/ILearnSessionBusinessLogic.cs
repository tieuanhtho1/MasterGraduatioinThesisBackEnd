using WebAPI.Models.DTOs.LearnSession;

namespace WebAPI.BusinessLogic.LearnSession;

public interface ILearnSessionBusinessLogic
{
    Task<List<Models.FlashCard>> GetLearnSessionFlashCardsAsync(int collectionId, int count = 10);
    Task<bool> UpdateFlashCardScoresAsync(List<FlashCardScoreUpdate> scoreUpdates);
}
