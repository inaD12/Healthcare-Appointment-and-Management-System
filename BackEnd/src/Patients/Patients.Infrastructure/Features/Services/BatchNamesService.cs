using MassTransit;
using Shared.Application.IntegrationEvents;
using Shared.Domain.Abstractions;
using Shared.Domain.Models;

namespace Patients.Infrastructure.Features.Services;

public class BatchNamesService(IRequestClient<GeBatchUserNamesRequest> requestClient): IBatchNamesService
{
    public async Task<GetUsersByIdsResponse> GetUsersNamesByIdsAsync(IEnumerable<string> userIds, CancellationToken cancellationToken = default)
    {
        var request = new GeBatchUserNamesRequest(userIds);
        
        var response = await requestClient.GetResponse<GetUsersByIdsResponse>(request, cancellationToken);
        
        return response.Message;
    }
}