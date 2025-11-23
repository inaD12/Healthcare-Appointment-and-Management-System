using System.Net.Http.Json;
using System.Text.Json;
using Doctors.Domain.Abstractions;
using Doctors.Domain.Options;
using Microsoft.Extensions.Options;
using Pgvector;

namespace Doctors.Infrastructure.Features.Clients;

public class EmbeddingClient(HttpClient client, IOptions<OllamaOptions> options): IEmbeddingClient
{
    private readonly OllamaOptions _options = options.Value;
    
    public async Task<Vector> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default)
    {
        var payload = new
        {
            model = _options.Model,
            input = text
        };

        var response = await client
            .PostAsJsonAsync($"/api/embed", payload, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new HttpRequestException($"Ollama API error ({(int)response.StatusCode}): {error}");
        }

        using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
        var json = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);

        var floats = json.RootElement
            .GetProperty("embeddings")[0]
            .EnumerateArray()
            .Select(e => e.GetSingle())
            .ToArray();

        return new Vector(floats);
    }
}