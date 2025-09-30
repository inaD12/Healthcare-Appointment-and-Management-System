using MassTransit;
using Shared.Application.Authorization;
using Shared.Application.IntegrationEvents;
using Shared.Domain.Results;

namespace Users.Application.Features.Users.Consumers;

public class GetUserPermissionsRequestConsumer(IPermissionService permissionService) : IConsumer<GetUserPermissionsRequest>
{
    public async Task Consume(ConsumeContext<GetUserPermissionsRequest> context)
    {
        var result = await permissionService.GetUserPermissionsAsync(context.Message.IdentityId);

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