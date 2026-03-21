using System.IO;
using System.Threading.Tasks;

namespace WebAPI.Services.AIGeneration;

public interface IDocumentParserService
{
    Task<string> ExtractTextAsync(Stream fileStream, string fileName);
}
