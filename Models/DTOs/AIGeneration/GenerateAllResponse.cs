using System.Collections.Generic;

namespace WebAPI.Models.DTOs.AIGeneration;

public class GenerateAllResponse
{
    public AICollectionDto Collection { get; set; }
    public List<AIFlashCardDto> FlashCards { get; set; }
    public AIMindMapDto MindMap { get; set; }
}
