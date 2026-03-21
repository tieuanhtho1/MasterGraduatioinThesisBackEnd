using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Packaging;

using UglyToad.PdfPig;
using DocumentFormat.OpenXml.Presentation;

namespace WebAPI.Services.AIGeneration;

public class DocumentParserService : IDocumentParserService
{
    public Task<string> ExtractTextAsync(Stream fileStream, string fileName)
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        
        string result = ext switch
        {
            ".pdf" => ExtractPdf(fileStream),
            ".docx" => ExtractDocx(fileStream),
            ".pptx" => ExtractPptx(fileStream),
            ".doc" => ExtractDoc(fileStream),
            ".ppt" => ExtractPpt(fileStream),
            _ => throw new NotSupportedException($"Unsupported file format: {ext}")
        };

        return Task.FromResult(result);
    }

    private string ExtractPdf(Stream stream)
    {
        using var pdfDocument = PdfDocument.Open(stream);
        var sb = new StringBuilder();
        foreach (var page in pdfDocument.GetPages())
        {
            sb.AppendLine(page.Text);
        }
        return sb.ToString();
    }

    private string ExtractDocx(Stream stream)
    {
        using var wordDoc = WordprocessingDocument.Open(stream, false);
        var body = wordDoc.MainDocumentPart?.Document.Body;
        return body?.InnerText ?? string.Empty;
    }

    private string ExtractPptx(Stream stream)
    {
        using var pptDoc = PresentationDocument.Open(stream, false);
        var presentationPart = pptDoc.PresentationPart;
        if (presentationPart == null) return string.Empty;

        var sb = new StringBuilder();
        foreach (var slideId in presentationPart.Presentation.SlideIdList.Elements<SlideId>())
        {
            if (presentationPart.GetPartById(slideId.RelationshipId) is SlidePart slidePart && slidePart.Slide != null)
            {
                foreach (var text in slidePart.Slide.Descendants<DocumentFormat.OpenXml.Drawing.Text>())
                {
                    sb.AppendLine(text.Text);
                }
            }
        }
        return sb.ToString();
    }

    private string ExtractDoc(Stream stream)
    {
        throw new NotSupportedException("Legacy .doc format is not supported. Please use .docx instead.");
    }

    private string ExtractPpt(Stream stream)
    {
        throw new NotSupportedException("Legacy .ppt format is not supported. Please use .pptx instead.");
    }
}
