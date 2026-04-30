using MassTransit;
using Shared.Application.IntegrationEvents;
using Shared.Domain.Abstractions;

namespace Users.Infrastructure.Features.Consumers;

public class GetBatchUserNamesRequestConsumer(IBatchNamesService namesService) : IConsumer<GeBatchUserNamesRequest>
{
    public async Task Consume(ConsumeContext<GeBatchUserNamesRequest> context)
    {
        var result = await namesService.GetUsersNamesByIdsAsync(context.Message.UserIds, context.CancellationToken);

        await context.RespondAsync(result);
    }
}   