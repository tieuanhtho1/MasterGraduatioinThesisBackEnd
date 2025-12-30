using WebAPI.Models;
using WebAPI.Models.DTOs.MindMap;
using WebAPI.Services.Mindmap;

namespace WebAPI.BusinessLogic.Mindmap
{
    public class MindMapBusinessLogic : IMindMapBusinessLogic
    {
        private readonly IMindMapService _mindMapService;

        public MindMapBusinessLogic(IMindMapService mindMapService)
        {
            _mindMapService = mindMapService;
        }

        // MindMap operations
        public async Task<MindMapResponseDto?> GetMindMapByIdAsync(int mindMapId, int userId)
        {
            var mindMap = await _mindMapService.GetMindMapByIdAsync(mindMapId, userId);
            if (mindMap == null) return null;

            return MapToResponseDto(mindMap);
        }

        public async Task<List<MindMapResponseDto>> GetUserMindMapsAsync(int userId)
        {
            var mindMaps = await _mindMapService.GetUserMindMapsAsync(userId);
            return mindMaps.Select(MapToResponseDto).ToList();
        }

        public async Task<MindMapResponseDto> CreateMindMapAsync(CreateMindMapDto dto, int userId)
        {
            var mindMap = new MindMap
            {
                Name = dto.Name,
                Description = dto.Description,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            var createdMindMap = await _mindMapService.CreateMindMapAsync(mindMap);
            return MapToResponseDto(createdMindMap);
        }

        public async Task<MindMapResponseDto?> UpdateMindMapAsync(int mindMapId, UpdateMindMapDto dto, int userId)
        {
            var updatedMindMap = await _mindMapService.UpdateMindMapAsync(mindMapId, userId, dto);
            if (updatedMindMap == null) return null;

            return MapToResponseDto(updatedMindMap);
        }

        public async Task<bool> DeleteMindMapAsync(int mindMapId, int userId)
        {
            return await _mindMapService.DeleteMindMapAsync(mindMapId, userId);
        }

        public async Task<FullMindMapResponseDto?> GetFullMindMapAsync(int mindMapId, int userId)
        {
            var mindMap = await _mindMapService.GetFullMindMapAsync(mindMapId, userId);
            if (mindMap == null) return null;

            return new FullMindMapResponseDto
            {
                Id = mindMap.Id,
                Name = mindMap.Name,
                Description = mindMap.Description,
                UserId = mindMap.UserId,
                CreatedAt = mindMap.CreatedAt,
                UpdatedAt = mindMap.UpdatedAt,
                Nodes = mindMap.Nodes.Select(n => new MindMapNodeWithFlashCardDto
                {
                    Id = n.Id,
                    MindMapId = n.MindMapId,
                    ParentNodeId = n.ParentNodeId,
                    PositionX = n.PositionX,
                    PositionY = n.PositionY,
                    Color = n.Color,
                    HideChildren = n.HideChildren,
                    CreatedAt = n.CreatedAt,
                    UpdatedAt = n.UpdatedAt,
                    FlashCard = new FlashCardInfo
                    {
                        Id = n.FlashCard.Id,
                        Term = n.FlashCard.Term,
                        Definition = n.FlashCard.Definition,
                        Score = n.FlashCard.Score,
                        LearnCount = n.FlashCard.TimesLearned,
                        CollectionId = n.FlashCard.FlashCardCollectionId,
                        CollectionName = n.FlashCard.FlashCardCollection.Title
                    }
                }).ToList()
            };
        }

        // MindMapNode operations
        public async Task<MindMapNodeResponseDto?> GetNodeByIdAsync(int nodeId, int userId)
        {
            var node = await _mindMapService.GetNodeByIdAsync(nodeId, userId);
            if (node == null) return null;

            return MapToNodeResponseDto(node);
        }

        public async Task<MindMapNodeResponseDto> CreateNodeAsync(int mindMapId, CreateMindMapNodeDto dto, int userId)
        {
            // Validate mindmap exists and belongs to user
            var mindMap = await _mindMapService.GetMindMapByIdAsync(mindMapId, userId);
            if (mindMap == null)
                throw new UnauthorizedAccessException("MindMap not found or access denied");

            // Validate flashcard exists and belongs to user
            var hasAccess = await _mindMapService.ValidateFlashCardAccessAsync(dto.FlashCardId, userId);
            if (!hasAccess)
                throw new UnauthorizedAccessException("FlashCard not found or access denied");

            // Validate parent node if provided
            if (dto.ParentNodeId.HasValue)
            {
                var parentNode = await _mindMapService.GetNodeByIdAsync(dto.ParentNodeId.Value, userId);
                if (parentNode == null || parentNode.MindMapId != mindMapId)
                    throw new InvalidOperationException("Parent node not found or doesn't belong to the same mindmap");
            }

            var node = new MindMapNode
            {
                MindMapId = mindMapId,
                FlashCardId = dto.FlashCardId,
                ParentNodeId = dto.ParentNodeId,
                PositionX = dto.PositionX,
                PositionY = dto.PositionY,
                Color = dto.Color,
                HideChildren = dto.HideChildren,
                CreatedAt = DateTime.UtcNow
            };

            var createdNode = await _mindMapService.CreateNodeAsync(node);
            
            // Update mindmap's UpdatedAt
            await _mindMapService.UpdateMindMapAsync(mindMapId, userId, new UpdateMindMapDto());

            return MapToNodeResponseDto(createdNode);
        }

        public async Task<MindMapNodeResponseDto?> UpdateNodeAsync(int nodeId, UpdateMindMapNodeDto dto, int userId)
        {
            var updatedNode = await _mindMapService.UpdateNodeAsync(nodeId, userId, dto);
            if (updatedNode == null) return null;

            // Update mindmap's UpdatedAt
            await _mindMapService.UpdateMindMapAsync(updatedNode.MindMapId, userId, new UpdateMindMapDto());

            return MapToNodeResponseDto(updatedNode);
        }

        public async Task<bool> DeleteNodeAsync(int nodeId, int userId)
        {
            var node = await _mindMapService.GetNodeByIdAsync(nodeId, userId);
            if (node == null) return false;

            var mindMapId = node.MindMapId;
            var result = await _mindMapService.DeleteNodeAsync(nodeId, userId);

            if (result)
            {
                // Update mindmap's UpdatedAt
                await _mindMapService.UpdateMindMapAsync(mindMapId, userId, new UpdateMindMapDto());
            }

            return result;
        }

        // Helper methods
        private MindMapResponseDto MapToResponseDto(MindMap mindMap)
        {
            return new MindMapResponseDto
            {
                Id = mindMap.Id,
                Name = mindMap.Name,
                Description = mindMap.Description,
                UserId = mindMap.UserId,
                CreatedAt = mindMap.CreatedAt,
                UpdatedAt = mindMap.UpdatedAt,
                NodeCount = mindMap.Nodes?.Count ?? 0
            };
        }

        private MindMapNodeResponseDto MapToNodeResponseDto(MindMapNode node)
        {
            return new MindMapNodeResponseDto
            {
                Id = node.Id,
                MindMapId = node.MindMapId,
                FlashCardId = node.FlashCardId,
                ParentNodeId = node.ParentNodeId,
                PositionX = node.PositionX,
                PositionY = node.PositionY,
                Color = node.Color,
                HideChildren = node.HideChildren,
                CreatedAt = node.CreatedAt,
                UpdatedAt = node.UpdatedAt
            };
        }
    }
}
