using Doctors.Domain.Abstractions.Repositories;
using MassTransit;
using Shared.Application.IntegrationEvents;
using Shared.Domain.Abstractions;

namespace Doctors.Infrastructure.Features.Consumers;

public sealed class DoctorAverageRatingUpdatedIntegrationEventConsumer(
    IDoctorRepository doctorRepository,
    IUnitOfWork unitOfWork) 
    : IConsumer<DoctorAverageRatingUpdatedIntegrationEvent>
{
    
    public async Task Consume(ConsumeContext<DoctorAverageRatingUpdatedIntegrationEvent> context)
    {
        var doctor = await doctorRepository.GetByUserIdAsync(context.Message.DoctorId);
        if (doctor is null)
            return;
        
        doctor.ChangeAverageRating(context.Message.NewAverageRating);
        doctor.ChangeRatingsCount(context.Message.NewRatingsCount);

        await unitOfWork.SaveChangesAsync(context.CancellationToken);
    }
}
