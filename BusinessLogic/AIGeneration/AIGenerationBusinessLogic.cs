using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebAPI.BusinessLogic.UserApiKey;
using WebAPI.Data;
using WebAPI.Models;
using WebAPI.Models.DTOs.AIGeneration;
using WebAPI.Services.AIGeneration;

namespace WebAPI.BusinessLogic.AIGeneration;

public class AIGenerationBusinessLogic : IAIGenerationBusinessLogic
{
    private readonly ApplicationDbContext _context;
    private readonly IDocumentParserService _documentParser;
    private readonly IAIService _aiService;
    private readonly IUserApiKeyBusinessLogic _userApiKeyBusinessLogic;

    public AIGenerationBusinessLogic(
        ApplicationDbContext context,
        IDocumentParserService documentParser,
        IAIService aiService,
        IUserApiKeyBusinessLogic userApiKeyBusinessLogic)
    {
        _context = context;
        _documentParser = documentParser;
        _aiService = aiService;
        _userApiKeyBusinessLogic = userApiKeyBusinessLogic;
    }

    public async Task<GenerateFlashCardsResponse> GenerateFlashCardsAsync(GenerateFlashCardsRequest request)
    {
        var ext = Path.GetExtension(request.File.FileName).ToLowerInvariant();
        var allowedExtensions = new[] { ".docx", ".pptx", ".doc", ".ppt", ".pdf" };
        if (!allowedExtensions.Contains(ext))
        {
            throw new ArgumentException("Unsupported file format. Supported: .docx, .pptx, .doc, .ppt, .pdf");
        }

        using var stream = request.File.OpenReadStream();
        string text = await _documentParser.ExtractTextAsync(stream, request.File.FileName);

        if (string.IsNullOrWhiteSpace(text))
        {
            throw new InvalidOperationException("Could not extract text from the uploaded file");
        }

        var apiKey = await _userApiKeyBusinessLogic.GetApiKeyForProviderAsync(request.UserId, request.Provider);
        var aiResult = await _aiService.GenerateFlashCardsFromTextAsync(text, request.Provider, request.Model, apiKey);

        var collection = new Models.FlashCardCollection
        {
            Title = aiResult.Title ?? "Generated Collection",
            Description = aiResult.Description ?? "AI Generated Flashcards",
            UserId = request.UserId,
            ParentId = request.ParentCollectionId == 0 ? null : request.ParentCollectionId
        };

        _context.FlashCardCollections.Add(collection);
        await _context.SaveChangesAsync();

        var flashCards = new List<Models.FlashCard>();
        foreach (var fc in aiResult.Flashcards)
        {
            flashCards.Add(new Models.FlashCard
            {
                Term = fc.Term,
                Definition = fc.Definition,
                Score = 0,
                TimesLearned = 0,
                FlashCardCollectionId = collection.Id
            });
        }
        _context.FlashCards.AddRange(flashCards);
        await _context.SaveChangesAsync();

        return new GenerateFlashCardsResponse
        {
            Collection = new AICollectionDto
            {
                Id = collection.Id,
                Title = collection.Title,
                Description = collection.Description,
                ParentId = collection.ParentId,
                UserId = collection.UserId
            },
            FlashCards = flashCards.Select(f => new AIFlashCardDto
            {
                Id = f.Id,
                Term = f.Term,
                Definition = f.Definition,
                Score = f.Score,
                FlashCardCollectionId = f.FlashCardCollectionId
            }).ToList()
        };
    }

    public async Task<GenerateMindMapResponse> GenerateMindMapAsync(GenerateMindMapRequest request)
    {
        var collection = await _context.FlashCardCollections
            .Include(c => c.FlashCards)
            .FirstOrDefaultAsync(c => c.Id == request.CollectionId);

        if (collection == null)
            throw new KeyNotFoundException("FlashCard collection not found");

        if (!collection.FlashCards.Any())
            throw new InvalidOperationException("Collection has no flashcards to generate a mind map from");

        var flashcardsList = collection.FlashCards.Select(f => new { f.Term, f.Definition }).ToList();
        var flashcardsJson = JsonSerializer.Serialize(flashcardsList);

        var apiKey = await _userApiKeyBusinessLogic.GetApiKeyForProviderAsync(request.UserId, request.Provider);
        var aiResult = await _aiService.GenerateMindMapFromFlashCardsAsync(flashcardsJson, request.Provider, request.Model, apiKey);

        var mindMap = new Models.MindMap
        {
            Title = aiResult.Title ?? "Generated Mind Map",
            Description = aiResult.Description ?? "AI Generated Mind Map",
            UserId = request.UserId,
            FlashCardCollectionId = collection.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.MindMaps.Add(mindMap);
        await _context.SaveChangesAsync();

        var nodes = new List<MindMapNode>();
        var flashcardArray = collection.FlashCards.ToArray();
        var aiNodeIdToEntity = new Dictionary<int, MindMapNode>();

        foreach (var aiNode in aiResult.Nodes)
        {
            int flashcardId;

            if (aiNode.FlashcardIndex == -1)
            {
                var newFc = new Models.FlashCard
                {
                    Term = aiNode.Label ?? "Category",
                    Definition = "Category",
                    Score = 0,
                    TimesLearned = 0,
                    FlashCardCollectionId = collection.Id
                };
                _context.FlashCards.Add(newFc);
                await _context.SaveChangesAsync();
                flashcardId = newFc.Id;
            }
            else
            {
                if (aiNode.FlashcardIndex >= 0 && aiNode.FlashcardIndex < flashcardArray.Length)
                {
                    flashcardId = flashcardArray[aiNode.FlashcardIndex].Id;
                }
                else
                {
                    var newFc = new Models.FlashCard
                    {
                        Term = aiNode.Label ?? "Concept",
                        Definition = "Concept",
                        Score = 0,
                        TimesLearned = 0,
                        FlashCardCollectionId = collection.Id
                    };
                    _context.FlashCards.Add(newFc);
                    await _context.SaveChangesAsync();
                    flashcardId = newFc.Id;
                }
            }

            var node = new MindMapNode
            {
                MindMapId = mindMap.Id,
                FlashCardId = flashcardId,
                Color = "#ffffff",
                HideChildren = false,
                PositionX = 0,
                PositionY = 0
            };

            nodes.Add(node);
            aiNodeIdToEntity[aiNode.Id] = node;
        }

        _context.MindMapNodes.AddRange(nodes);
        await _context.SaveChangesAsync();

        var edges = new List<MindMapEdge>();
        foreach (var aiNode in aiResult.Nodes)
        {
            if (aiNode.ParentNodeId.HasValue && aiNodeIdToEntity.ContainsKey(aiNode.ParentNodeId.Value))
            {
                var sourceNode = aiNodeIdToEntity[aiNode.ParentNodeId.Value];
                var targetNode = aiNodeIdToEntity[aiNode.Id];

                edges.Add(new MindMapEdge
                {
                    MindMapId = mindMap.Id,
                    SourceNodeId = sourceNode.Id,
                    TargetNodeId = targetNode.Id,
                    SourceHandle = "bottom",
                    TargetHandle = "top"
                });
            }
        }

        if (edges.Any())
        {
            _context.MindMapEdges.AddRange(edges);
            await _context.SaveChangesAsync();
        }

        ApplyTreeLayout(aiResult.Nodes, aiNodeIdToEntity);
        await _context.SaveChangesAsync();

        var finalFlashcards = await _context.FlashCards
            .Where(f => f.FlashCardCollectionId == collection.Id)
            .ToDictionaryAsync(f => f.Id);

        return new GenerateMindMapResponse
        {
            MindMap = new AIMindMapDto
            {
                Id = mindMap.Id,
                Title = mindMap.Title,
                Description = mindMap.Description,
                UserId = mindMap.UserId,
                FlashCardCollectionId = mindMap.FlashCardCollectionId,
                CollectionTitle = collection.Title,
                CreatedAt = mindMap.CreatedAt,
                UpdatedAt = mindMap.UpdatedAt,
                Nodes = nodes.Select(n => new AIMindMapNodeDto
                {
                    Id = n.Id,
                    PositionX = n.PositionX,
                    PositionY = n.PositionY,
                    Color = n.Color,
                    FlashCardId = n.FlashCardId,
                    FlashCard = new AIFlashCardDto
                    {
                        Id = n.FlashCardId,
                        Term = finalFlashcards[n.FlashCardId].Term,
                        Definition = finalFlashcards[n.FlashCardId].Definition,
                        Score = finalFlashcards[n.FlashCardId].Score,
                        FlashCardCollectionId = collection.Id
                    }
                }).ToList(),
                Edges = edges.Select(e => new AIMindMapEdgeDto
                {
                    Id = e.Id,
                    SourceNodeId = e.SourceNodeId,
                    TargetNodeId = e.TargetNodeId,
                    SourceHandle = e.SourceHandle,
                    TargetHandle = e.TargetHandle
                }).ToList()
            }
        };
    }

    public async Task<GenerateAllResponse> GenerateAllAsync(GenerateAllRequest request)
    {
        var flashCardRes = await GenerateFlashCardsAsync(new GenerateFlashCardsRequest
        {
            File = request.File,
            ParentCollectionId = request.ParentCollectionId,
            UserId = request.UserId,
            Provider = request.Provider,
            Model = request.Model
        });

        var mindMapRes = await GenerateMindMapAsync(new GenerateMindMapRequest
        {
            CollectionId = flashCardRes.Collection.Id,
            UserId = request.UserId,
            Provider = request.Provider,
            Model = request.Model
        });

        return new GenerateAllResponse
        {
            Collection = flashCardRes.Collection,
            FlashCards = flashCardRes.FlashCards,
            MindMap = mindMapRes.MindMap
        };
    }

    private void ApplyTreeLayout(List<AIGeneratedMindMapNode> aiNodes, Dictionary<int, MindMapNode> aiNodeIdToEntity)
    {
        var rootNodes = aiNodes.Where(n => n.ParentNodeId == null).ToList();
        if (!rootNodes.Any()) return;

        var childrenMap = aiNodes.Where(n => n.ParentNodeId != null)
            .GroupBy(n => n.ParentNodeId.Value)
            .ToDictionary(g => g.Key, g => g.ToList());

        int currentY = 0;
        var queue = new Queue<List<AIGeneratedMindMapNode>>();
        queue.Enqueue(rootNodes);

        while (queue.Any())
        {
            var levelNodes = queue.Dequeue();
            int totalWidth = (levelNodes.Count - 1) * 200;
            int startX = 400 - (totalWidth / 2);

            var nextLevel = new List<AIGeneratedMindMapNode>();

            for (int i = 0; i < levelNodes.Count; i++)
            {
                var aiNode = levelNodes[i];
                if (aiNodeIdToEntity.TryGetValue(aiNode.Id, out var entity))
                {
                    entity.PositionX = startX + (i * 200);
                    entity.PositionY = currentY;
                }

                if (childrenMap.TryGetValue(aiNode.Id, out var children))
                {
                    nextLevel.AddRange(children);
                }
            }

            if (nextLevel.Any())
            {
                queue.Enqueue(nextLevel);
                currentY += 150;
            }
        }
    }
}
