using System.Net;
using Shared.Domain.Results;
using Users.Domain.Auth.Abstractions;
using Users.Domain.Auth.Models;
using Users.Domain.Utilities;

namespace Users.Infrastructure.Features.Identity;

internal sealed class IdentityProviderService(KeyCloakClient keyCloakClient)
    : IIdentityProviderService
{
    private const string PasswordCredentialType = "password";

    public async Task<Result<string>> RegisterUserAsync(UserModel user, CancellationToken cancellationToken = default)
    {
        var userRepresentation = new UserRepresentation(
            user.Email,
            user.Email,
            user.FirstName,
            user.LastName,
            false,
            true,
            [new CredentialRepresentation(PasswordCredentialType, user.Password, false)]);

        try
        {
            string identityId = await keyCloakClient.RegisterUserAsync(userRepresentation, cancellationToken);

            return Result<string>.Success(identityId);
        }
        catch (HttpRequestException exception) when (exception.StatusCode == HttpStatusCode.Conflict)
        {
            return Result<string>.Failure(ResponseList.EmailTaken);
        }
    }
}
