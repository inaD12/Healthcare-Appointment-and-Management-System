using Shared.Domain.Results;
using Users.Domain.Infrastructure.Auth.Models;

namespace Users.Domain.Infrastructure.Auth.Abstractions;

public interface IIdentityProviderService
{
    Task<Result<string>> RegisterUserAsync(UserModel user, CancellationToken cancellationToken = default);
}
