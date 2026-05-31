using Pgvector;

namespace Doctors.Application.Features.Doctors.Abstractions;

public interface IEmbeddingClient
{
    Task<Vector> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default);
}