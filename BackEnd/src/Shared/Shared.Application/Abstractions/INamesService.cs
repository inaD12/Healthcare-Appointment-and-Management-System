using Shared.Application.Models;
using Shared.Domain.Results;

namespace Shared.Application.Abstractions;

public interface INamesService
{
    Task<Result<NamesResponse>> GetUserNamesAsync(string userId, CancellationToken cancellationToken = default);
}