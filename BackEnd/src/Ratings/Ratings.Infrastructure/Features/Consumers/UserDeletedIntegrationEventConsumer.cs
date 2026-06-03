using MassTransit;
using Ratings.Application.Features.Ratings.Abstractions;
using Shared.Application.Abstractions;
using Shared.Application.IntegrationEvents;
using Shared.Domain.Enums;

namespace Ratings.Infrastructure.Features.Consumers;

public sealed class UserDeletedIntegrationEventConsumer(
    IRatingRepository  ratingRepository,
    IDoctorRatingStatsRepository  doctorRatingStatsRepository,
    IUnitOfWork unitOfWork) 
    : IConsumer<UserDeletedIntegrationEvent>
{
    public async Task Consume(ConsumeContext<UserDeletedIntegrationEvent> context)
    {
        var msg = context.Message;
        if(!msg.Roles.Contains(Roles.Doctor))
            return;

        ratingRepository.DeleteByDoctorId(msg.Id);
        await doctorRatingStatsRepository.DeleteByIdAsync(msg.Id, context.CancellationToken);
        
        await unitOfWork.SaveChangesAsync(context.CancellationToken);
    }
}
