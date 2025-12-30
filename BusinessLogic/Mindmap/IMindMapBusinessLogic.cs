using WebAPI.Models.DTOs.MindMap;

namespace WebAPI.BusinessLogic.Mindmap
{
    public interface IMindMapBusinessLogic
    {
        // MindMap operations
        Task<MindMapResponseDto?> GetMindMapByIdAsync(int mindMapId, int userId);
        Task<List<MindMapResponseDto>> GetUserMindMapsAsync(int userId);
        Task<MindMapResponseDto> CreateMindMapAsync(CreateMindMapDto dto, int userId);
        Task<MindMapResponseDto?> UpdateMindMapAsync(int mindMapId, UpdateMindMapDto dto, int userId);
        Task<bool> DeleteMindMapAsync(int mindMapId, int userId);
        Task<FullMindMapResponseDto?> GetFullMindMapAsync(int mindMapId, int userId);

        // MindMapNode operations
        Task<MindMapNodeResponseDto?> GetNodeByIdAsync(int nodeId, int userId);
        Task<MindMapNodeResponseDto> CreateNodeAsync(int mindMapId, CreateMindMapNodeDto dto, int userId);
        Task<MindMapNodeResponseDto?> UpdateNodeAsync(int nodeId, UpdateMindMapNodeDto dto, int userId);
        Task<bool> DeleteNodeAsync(int nodeId, int userId);
    }
}
