using Doctors.Domain.Abstractions.Repositories;
using MassTransit;
using Shared.Application.IntegrationEvents;

namespace Doctors.Infrastructure.Features.Consumers;

public sealed class DoctorAverageRatingUpdatedIntegrationEventConsumer(
    IDoctorRepository doctorRepository) 
    : IConsumer<DoctorAverageRatingUpdatedIntegrationEvent>
{
    
    public async Task Consume(ConsumeContext<DoctorAverageRatingUpdatedIntegrationEvent> context)
    {
        var doctor = await doctorRepository.GetByIdAsync(context.Message.DoctorId);
        if (doctor is null)
            return;
        
        doctor.ChangeAverageRating(context.Message.NewAverageRating);
        doctor.ChangeRatingsCount(context.Message.NewRatingsCount);
    }
}
