using MassTransit;
using Shared.Application.IntegrationEvents;
using Shared.Domain.Abstractions;
using Shared.Domain.Models;
using Shared.Domain.Responses;
using Shared.Domain.Results;

namespace Appointments.Infrastructure.Features.Services;

public class RolesService(IRequestClient<GetUserRolesRequest> requestClient): IRolesService
{
    public async Task<Result<RolesResponse>> GetUserRolesAsync(string userId, CancellationToken cancellationToken = default)
    {
        //TODO: Cache
        
        var request = new GetUserRolesRequest(userId);
        
        Response<RolesResponse, Result> response =
            await requestClient.GetResponse<RolesResponse, Result>(request, cancellationToken);

        if (response.Is(out Response<Result>? errorResponse))
        {
            return Result<RolesResponse>.Failure(errorResponse.Message.Response);
        }
        
        if (response.Is(out Response<RolesResponse>? permissionResponse))
        {
            
            return Result<RolesResponse>.Success(permissionResponse.Message);
        }
        
        return Result<RolesResponse>.Failure(SharedResponses.EntityNotFound);
    }
}