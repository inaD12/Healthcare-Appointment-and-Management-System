using MassTransit;
using Shared.Application.IntegrationEvents;
using Shared.Domain.Abstractions;
using Shared.Domain.Responses;
using Shared.Domain.Results;
using Users.Domain.Infrastructure.Models;

namespace Doctors.Infrastructure.Features.Services;

public class NamesService(IRequestClient<GetUserNamesRequest> requestClient): INamesService
{
    public async Task<Result<NamesResponse>> GetUserNamesAsync(string userId)
    {
        var request = new GetUserNamesRequest(userId);
        
        Response<NamesResponse, Result> response =
            await requestClient.GetResponse<NamesResponse, Result>(request);

        if (response.Is(out Response<Result> errorResponse))
        {
            return Result<NamesResponse>.Failure(errorResponse.Message.Response);
        }
        
        if (response.Is(out Response<NamesResponse> permissionResponse))
        {
            
            return Result<NamesResponse>.Success(permissionResponse.Message);
        }
        
        return Result<NamesResponse>.Failure(SharedResponses.EntityNotFound);
    }
}