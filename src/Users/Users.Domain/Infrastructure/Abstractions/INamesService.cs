using Shared.Domain.Results;
using Users.Domain.Infrastructure.Models;

namespace Users.Domain.Infrastructure.Abstractions;

public interface INamesService
{
    Task<Result<NamesResponse>> GetUserNamesAsync(string userId);
}