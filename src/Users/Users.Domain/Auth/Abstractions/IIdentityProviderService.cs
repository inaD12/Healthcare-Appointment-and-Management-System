using Shared.Domain.Results;
using Users.Application.Features.Users.Identity;

namespace Users.Domain.Auth.Abstractions;

public interface IIdentityProviderService
{
    Task<Result<string>> RegisterUserAsync(UserModel user, CancellationToken cancellationToken = default);
}
