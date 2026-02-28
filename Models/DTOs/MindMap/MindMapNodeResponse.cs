namespace WebAPI.Models.DTOs.MindMap;

public class MindMapNodeResponse
{
    public int Id { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
    public string Color { get; set; } = "#ffffff";
    public bool HideChildren { get; set; }
    public int? ParentNodeId { get; set; }
    public int MindMapId { get; set; }
    public int FlashCardId { get; set; }

    // Flash card data for display
    public FlashCardInfo FlashCard { get; set; } = null!;
}

public class FlashCardInfo
{
    public int Id { get; set; }
    public string Term { get; set; } = string.Empty;
    public string Definition { get; set; } = string.Empty;
    public int Score { get; set; }
    public int TimesLearned { get; set; }
    public int FlashCardCollectionId { get; set; }
}
