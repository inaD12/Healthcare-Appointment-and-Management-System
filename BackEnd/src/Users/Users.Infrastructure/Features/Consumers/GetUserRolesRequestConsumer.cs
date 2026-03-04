using MassTransit;
using Shared.Application.Authorization;
using Shared.Application.IntegrationEvents;
using Shared.Domain.Abstractions;
using Shared.Domain.Results;

namespace Users.Infrastructure.Features.Consumers;

public class GetUserRolesRequestConsumer(IRolesService rolesService) : IConsumer<GetUserRolesRequest>
{
    public async Task Consume(ConsumeContext<GetUserRolesRequest> context)
    {
        var result = await rolesService.GetUserRolesAsync(context.Message.UserId);

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