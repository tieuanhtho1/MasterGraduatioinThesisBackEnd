namespace WebAPI.Services.LearnSession;

public interface ILearnSessionService
{
    Task<List<Models.FlashCard>> GetFlashCardsFromCollectionTreeAsync(int collectionId);
    Task<bool> UpdateFlashCardScoresAsync(Dictionary<int, int> scoreModifications);
}
