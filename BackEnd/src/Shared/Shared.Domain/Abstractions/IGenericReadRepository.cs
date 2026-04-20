namespace Shared.Domain.Abstractions;

public interface IGenericReadRepository<T> where T : class
{
	Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
}