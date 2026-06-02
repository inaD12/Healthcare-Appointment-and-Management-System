using Shared.Domain.Results;
using Users.Application.Features.Users.Models;

namespace Users.Application.Features.Users.Abstractions;

public interface IIdentityProviderService
{
    Task<Result<string>> RegisterUserAsync(UserModel user, CancellationToken cancellationToken = default);
}
