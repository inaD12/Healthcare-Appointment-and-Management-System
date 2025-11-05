using Pgvector;

namespace Doctors.Domain.Infrastructure.Abstractions;

public interface IEmbeddingClient
{
    Task<Vector> GenerateEmbeddingAsync(string text, CancellationToken cancellationToken = default);
}