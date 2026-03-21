using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Services.AIGeneration;

public class AIGeneratedFlashCardsResult
{
    public string Title { get; set; }
    public string Description { get; set; }
    public List<AIGeneratedFlashCard> Flashcards { get; set; }
}

public class AIGeneratedFlashCard
{
    public string Term { get; set; }
    public string Definition { get; set; }
}

public class AIGeneratedMindMapResult
{
    public string Title { get; set; }
    public string Description { get; set; }
    public List<AIGeneratedMindMapNode> Nodes { get; set; }
}

public class AIGeneratedMindMapNode
{
    public int Id { get; set; }
    public string Label { get; set; }
    public int FlashcardIndex { get; set; }
    public int? ParentNodeId { get; set; }
}

public interface IAIService
{
    Task<AIGeneratedFlashCardsResult> GenerateFlashCardsFromTextAsync(string text, string provider, string model, string apiKey);
    Task<AIGeneratedMindMapResult> GenerateMindMapFromFlashCardsAsync(string flashcardsJson, string provider, string model, string apiKey);
}
