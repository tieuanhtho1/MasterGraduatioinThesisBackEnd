using System.Threading.Tasks;
using WebAPI.Models.DTOs.AIGeneration;

namespace WebAPI.BusinessLogic.AIGeneration;

public interface IAIGenerationBusinessLogic
{
    Task<GenerateFlashCardsResponse> GenerateFlashCardsAsync(GenerateFlashCardsRequest request);
    Task<GenerateMindMapResponse> GenerateMindMapAsync(GenerateMindMapRequest request);
    Task<GenerateAllResponse> GenerateAllAsync(GenerateAllRequest request);
}
