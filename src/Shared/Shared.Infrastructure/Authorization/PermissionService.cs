using MassTransit;
using Shared.Application.Authorization;
using Shared.Application.IntegrationEvents;
using Shared.Domain.Responses;
using Shared.Domain.Results;

namespace Shared.Infrastructure.Authorization;

//TODO: add Redis
public sealed class PermissionService(
    IRequestClient<GetUserPermissionsRequest/*,
    ICacheService cacheService*/> requestClient): IPermissionService
{
    //private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(5);
    
    public async Task<Result<PermissionsResponse>> GetUserPermissionsAsync(string identityId, CancellationToken cancellationToken = default)
    {
        /*var permissionsResponse = await cacheService.GetAsync<PermissionsResponse>(CreateCacheKey(identityId));

        if (permissionsResponse is not null)
        {
            return Result<PermissionsResponse>.Success(permissionsResponse);
        }*/
        
        var request = new GetUserPermissionsRequest(identityId);
        
        Response<PermissionsResponse, Result> response =
            await requestClient.GetResponse<PermissionsResponse, Result>(request, cancellationToken);

        if (response.Is(out Response<Result> errorResponse))
        {
            return Result<PermissionsResponse>.Failure(errorResponse.Message.Response);
        }

        if (response.Is(out Response<PermissionsResponse> permissionResponse))
        {
            /*await cacheService.SetAsync(
                CreateCacheKey(identityId),
                permissionResponse.Message,
                CacheDuration);*/
            
            return Result<PermissionsResponse>.Success(permissionResponse.Message);
        }
        
        return Result<PermissionsResponse>.Failure(SharedResponses.EntityNotFound);
    }
    
   // private static string CreateCacheKey(string identityId)=> $"user-permissions:{identityId}";
}

