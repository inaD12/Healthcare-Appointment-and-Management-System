using Shared.Domain.Models;
using Shared.Domain.Results;

namespace Shared.Domain.Abstractions;

public interface INamesService
{
    Task<Result<NamesResponse>> GetUserNamesAsync(string userId, CancellationToken cancellationToken = default);
}