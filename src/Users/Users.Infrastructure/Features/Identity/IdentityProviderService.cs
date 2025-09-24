using System.Net;
using Microsoft.Extensions.Logging;
using Shared.Domain.Results;
using Users.Application.Features.Users.Identity;
using Users.Domain.Auth;
using Users.Domain.Auth.Abstractions;
using Users.Domain.Responses;

namespace Users.Infrastructure.Features.Identity;

internal sealed class IdentityProviderService(KeyCloakClient keyCloakClient, ILogger<IdentityProviderService> logger)
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
