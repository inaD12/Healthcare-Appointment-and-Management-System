using Shared.Domain.Results;
using Users.Domain.Infrastructure.Models;

namespace Shared.Domain.Abstractions;

public interface INamesService
{
    Task<Result<NamesResponse>> GetUserNamesAsync(string userId);
}