using MassTransit;
using Shared.Application.IntegrationEvents;
using Shared.Domain.Abstractions;
using Shared.Domain.Results;
using Users.Domain.Infrastructure.Abstractions;

namespace Users.Infrastructure.Features.Consumers;

public class GetUserNamesRequestConsumer(INamesService namesService) : IConsumer<GetUserNamesRequest>
{
    public async Task Consume(ConsumeContext<GetUserNamesRequest> context)
    {
        var result = await namesService.GetUserNamesAsync(context.Message.userId);

        if (result.IsSuccess)
        {
            await context.RespondAsync(result.Value!);
        }
        else
        {
            await context.RespondAsync(Result.Failure(result.Response));
        }
    }
}   