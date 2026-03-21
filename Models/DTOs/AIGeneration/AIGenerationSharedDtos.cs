using System;
using System.Collections.Generic;

namespace WebAPI.Models.DTOs.AIGeneration;

public class AICollectionDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int? ParentId { get; set; }
    public int UserId { get; set; }
}

public class AIFlashCardDto
{
    public int Id { get; set; }
    public string Term { get; set; }
    public string Definition { get; set; }
    public int Score { get; set; }
    public int FlashCardCollectionId { get; set; }
}

public class AIMindMapNodeDto
{
    public int Id { get; set; }
    public double PositionX { get; set; }
    public double PositionY { get; set; }
    public string Color { get; set; }
    public int? FlashCardId { get; set; }
    public AIFlashCardDto FlashCard { get; set; }
}

public class AIMindMapEdgeDto
{
    public int Id { get; set; }
    public int SourceNodeId { get; set; }
    public int TargetNodeId { get; set; }
    public string SourceHandle { get; set; }
    public string TargetHandle { get; set; }
}

public class AIMindMapDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int UserId { get; set; }
    public int FlashCardCollectionId { get; set; }
    public string CollectionTitle { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public List<AIMindMapNodeDto> Nodes { get; set; }
    public List<AIMindMapEdgeDto> Edges { get; set; }
}
