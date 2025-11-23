using Shared.Domain.Results;
using Users.Domain.Auth.Models;

namespace Users.Domain.Auth.Abstractions;

public interface IIdentityProviderService
{
    Task<Result<string>> RegisterUserAsync(UserModel user, CancellationToken cancellationToken = default);
}
