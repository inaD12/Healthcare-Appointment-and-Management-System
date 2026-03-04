using Pgvector;

namespace Doctors.Domain.Abstractions;

public interface IEmbeddingClient
{
    Task<Vector> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default);
}