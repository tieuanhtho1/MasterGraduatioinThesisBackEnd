// ============================================================
// MOCK SERVICE — for frontend integration testing.
// To restore the real service:
//   1. Delete everything from "// === MOCK ===" to the end of file
//   2. Uncomment the block below (remove the /* and */)
// ============================================================


using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Google.GenAI;
using Google.GenAI.Types;

namespace WebAPI.Services.AIGeneration;

public class AIService : IAIService
{

    private readonly HttpClient _httpClient;

    public AIService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<AIGeneratedFlashCardsResult> GenerateFlashCardsFromTextAsync(string text, string provider, string model, string apiKey)
    {
        string prompt = $@"
You are an expert educator. Analyze the following study material and create flashcards for effective learning.

Rules:
- Extract the most important concepts, terms, definitions, and facts
- Create between 10-30 flashcards depending on the content length
- Focus on key concepts that a student would need to memorize
- Organize from fundamental concepts to more advanced ones

Respond ONLY with valid JSON in this exact format:
{{
  ""title"": ""Collection title based on the content topic"",
  ""description"": ""Brief description of what these flashcards cover"",
  ""flashcards"": [
    {{ ""term"": ""Term or question"", ""definition"": ""Definition or answer"" }}
  ]
}}

Study material:
---
{text}
---
";

        string responseJson = await CallAIProviderAsync(prompt, provider, model, apiKey);
        responseJson = CleanJsonResponse(responseJson);

        return JsonSerializer.Deserialize<AIGeneratedFlashCardsResult>(responseJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    public async Task<AIGeneratedMindMapResult> GenerateMindMapFromFlashCardsAsync(string flashcardsJson, string provider, string model, string apiKey)
    {
        string prompt = $@"
            You are an expert at organizing knowledge visually. Given the following flashcards, create a hierarchical mind map structure that shows how the concepts relate to each other.

            Rules:
            - Create a central/root node that represents the main topic
            - Group related flashcards under parent category nodes
            - Each flashcard should be referenced by its index (0-based) in the provided list
            - Create logical parent-child relationships showing concept hierarchy
            - The root node should use flashcard index -1 (it will be a category node, not linked to a specific flashcard)
            - Category/group nodes should also use flashcard index -1
            - Leaf nodes should reference actual flashcard indices
            - Keep the tree depth between 2-4 levels

            Respond ONLY with valid JSON in this exact format:
            {{
            ""title"": ""Mind map title"",
            ""description"": ""Brief description"",
            ""nodes"": [
                {{ ""id"": 0, ""label"": ""Root Topic"", ""flashcardIndex"": -1, ""parentNodeId"": null }},
                {{ ""id"": 1, ""label"": ""Category A"", ""flashcardIndex"": -1, ""parentNodeId"": 0 }},
                {{ ""id"": 2, ""label"": ""Term from flashcard"", ""flashcardIndex"": 0, ""parentNodeId"": 1 }}
            ]
            }}

            Flashcards:
            ---
            {flashcardsJson}
            ---
            ";

        string responseJson = await CallAIProviderAsync(prompt, provider, model, apiKey);
        responseJson = CleanJsonResponse(responseJson);

        return JsonSerializer.Deserialize<AIGeneratedMindMapResult>(responseJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    private async Task<string> CallAIProviderAsync(string prompt, string provider, string model, string apiKey)
    {
        if (provider.Equals("openai", StringComparison.OrdinalIgnoreCase))
            return await CallOpenAIAsync(prompt, model, apiKey);
        else if (provider.Equals("gemini", StringComparison.OrdinalIgnoreCase))
            return await CallGeminiAsync(prompt, model, apiKey);
        else
            throw new ArgumentException("Invalid provider. Supported: openai, gemini");
    }

    private async Task<string> CallOpenAIAsync(string prompt, string model, string apiKey)
    {
        var requestBody = new
        {
            model = model,
            messages = new[] { new { role = "user", content = prompt } },
            temperature = 0.5
        };

        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/responses");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
        request.Content = JsonContent.Create(requestBody);

        var response = await _httpClient.SendAsync(request);
        var jsonResponse = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();

        using var doc = JsonDocument.Parse(jsonResponse);
        return doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();
    }

    private async Task<string> CallGeminiAsync(string prompt, string model, string apiKey)
    {
        // Use the official Google GenAI SDK instead of raw HttpClient.
        // Client is lightweight and safe to create per-request (no connection pool needed).
        using var client = new Client(apiKey: apiKey);

        var config = new GenerateContentConfig
        {
            Temperature = 0.5f
        };

        var response = await client.Models.GenerateContentAsync(
            model: model,
            contents: prompt,
            config: config);

        return response.Text
            ?? throw new InvalidOperationException("Gemini returned an empty response.");
    }

    private string CleanJsonResponse(string response)
    {
        if (string.IsNullOrWhiteSpace(response)) return string.Empty;
        response = response.Trim();
        if (response.StartsWith("```json", StringComparison.OrdinalIgnoreCase))
            response = response.Substring(7);
        else if (response.StartsWith("```"))
            response = response.Substring(3);
        if (response.EndsWith("```"))
            response = response.Substring(0, response.Length - 3);
        return response.Trim();
    }
}


// === MOCK ===
// Same class name, same interface — swap back by following the instructions above.

// using System.Collections.Generic;
// using System.Threading.Tasks;

// namespace WebAPI.Services.AIGeneration;

// public class AIService : IAIService
// {
//     // ------------------------------------------------------------------
//     // Input:  text        – any study-material string
//     //         provider    – "openai" | "gemini"
//     //         model       – e.g. "gpt-4o" | "gemini-1.5-pro"
//     // Output: hardcoded flashcard collection (simulates a real AI reply)
//     // ------------------------------------------------------------------
//     public Task<AIGeneratedFlashCardsResult> GenerateFlashCardsFromTextAsync(
//         string text, string provider, string model)
//     {
//         var result = new AIGeneratedFlashCardsResult
//         {
//             Title       = "[MOCK] Introduction to Machine Learning",
//             Description = "Key concepts and definitions from the provided study material (mocked response).",
//             Flashcards  = new List<AIGeneratedFlashCard>
//             {
//                 new() { Term = "Machine Learning",        Definition = "A subset of artificial intelligence that enables systems to learn from data without being explicitly programmed." },
//                 new() { Term = "Supervised Learning",     Definition = "A type of ML where the model is trained on labeled data — inputs paired with the correct outputs." },
//                 new() { Term = "Unsupervised Learning",   Definition = "A type of ML where the model finds hidden patterns in data without labeled responses." },
//                 new() { Term = "Reinforcement Learning",  Definition = "A type of ML where an agent learns by interacting with an environment and receiving rewards or penalties." },
//                 new() { Term = "Feature",                 Definition = "An individual measurable property or characteristic of the data used as input to a model." },
//                 new() { Term = "Label",                   Definition = "The output or target variable that the model is trained to predict." },
//                 new() { Term = "Training Set",            Definition = "The portion of data used to fit the model during training." },
//                 new() { Term = "Validation Set",          Definition = "Data used to tune hyperparameters and evaluate model performance during training." },
//                 new() { Term = "Test Set",                Definition = "Held-out data used to evaluate the final model performance after training is complete." },
//                 new() { Term = "Overfitting",             Definition = "When a model learns the training data too well, including noise, and performs poorly on new data." },
//                 new() { Term = "Underfitting",            Definition = "When a model is too simple to capture the underlying patterns in the data." },
//                 new() { Term = "Bias",                    Definition = "Error introduced by overly simplistic assumptions in the learning algorithm." },
//                 new() { Term = "Variance",                Definition = "Error from sensitivity to small fluctuations in the training data." },
//                 new() { Term = "Gradient Descent",        Definition = "An optimization algorithm that iteratively adjusts model parameters to minimize the loss function." },
//                 new() { Term = "Loss Function",           Definition = "A function that measures how far the model's predictions are from the actual values." },
//             }
//         };

//         return Task.FromResult(result);
//     }

//     // ------------------------------------------------------------------
//     // Input:  flashcardsJson – JSON string of flashcard list
//     //         provider       – "openai" | "gemini"
//     //         model          – e.g. "gpt-4o" | "gemini-1.5-pro"
//     // Output: hardcoded mind-map tree (simulates a real AI reply)
//     //         Node ids are sequential; parentNodeId=null means root.
//     //         flashcardIndex=-1 means category node (not a flashcard leaf).
//     // ------------------------------------------------------------------
//     public Task<AIGeneratedMindMapResult> GenerateMindMapFromFlashCardsAsync(
//         string flashcardsJson, string provider, string model)
//     {
//         var result = new AIGeneratedMindMapResult
//         {
//             Title       = "[MOCK] Machine Learning Mind Map",
//             Description = "Hierarchical structure of machine learning concepts derived from the flashcard set (mocked response).",
//             Nodes = new List<AIGeneratedMindMapNode>
//             {
//                 // Root
//                 new() { Id = 0,  Label = "Machine Learning",       FlashcardIndex = -1, ParentNodeId = null },

//                 // Category: Learning Types
//                 new() { Id = 1,  Label = "Learning Types",         FlashcardIndex = -1, ParentNodeId = 0 },
//                 new() { Id = 2,  Label = "Machine Learning",       FlashcardIndex = 0,  ParentNodeId = 1 },
//                 new() { Id = 3,  Label = "Supervised Learning",    FlashcardIndex = 1,  ParentNodeId = 1 },
//                 new() { Id = 4,  Label = "Unsupervised Learning",  FlashcardIndex = 2,  ParentNodeId = 1 },
//                 new() { Id = 5,  Label = "Reinforcement Learning", FlashcardIndex = 3,  ParentNodeId = 1 },

//                 // Category: Data Concepts
//                 new() { Id = 6,  Label = "Data Concepts",          FlashcardIndex = -1, ParentNodeId = 0 },
//                 new() { Id = 7,  Label = "Feature",                FlashcardIndex = 4,  ParentNodeId = 6 },
//                 new() { Id = 8,  Label = "Label",                  FlashcardIndex = 5,  ParentNodeId = 6 },
//                 new() { Id = 9,  Label = "Training Set",           FlashcardIndex = 6,  ParentNodeId = 6 },
//                 new() { Id = 10, Label = "Validation Set",         FlashcardIndex = 7,  ParentNodeId = 6 },
//                 new() { Id = 11, Label = "Test Set",               FlashcardIndex = 8,  ParentNodeId = 6 },

//                 // Category: Model Quality
//                 new() { Id = 12, Label = "Model Quality",          FlashcardIndex = -1, ParentNodeId = 0 },
//                 new() { Id = 13, Label = "Overfitting",            FlashcardIndex = 9,  ParentNodeId = 12 },
//                 new() { Id = 14, Label = "Underfitting",           FlashcardIndex = 10, ParentNodeId = 12 },
//                 new() { Id = 15, Label = "Bias",                   FlashcardIndex = 11, ParentNodeId = 12 },
//                 new() { Id = 16, Label = "Variance",               FlashcardIndex = 12, ParentNodeId = 12 },

//                 // Category: Optimization
//                 new() { Id = 17, Label = "Optimization",           FlashcardIndex = -1, ParentNodeId = 0 },
//                 new() { Id = 18, Label = "Gradient Descent",       FlashcardIndex = 13, ParentNodeId = 17 },
//                 new() { Id = 19, Label = "Loss Function",          FlashcardIndex = 14, ParentNodeId = 17 },
//             }
//         };

//         return Task.FromResult(result);
//     }
// }
