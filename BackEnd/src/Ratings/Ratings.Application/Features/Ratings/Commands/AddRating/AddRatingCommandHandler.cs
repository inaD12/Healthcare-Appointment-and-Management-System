using Ratings.Application.Features.Ratings.Mappers;
using Ratings.Application.Features.Ratings.Models;
using Ratings.Domain.Abstractions.Repositories;
using Ratings.Domain.Entities;
using Ratings.Domain.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Ratings.Application.Features.Ratings.Commands.AddRating;

public sealed class AddRatingCommandHandler(
    IRatingRepository ratingRepository,
    IDoctorRatingStatsRepository doctorRatingStatsRepository,
    IRateableAppointmentRepository rateableAppointmentRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<AddRatingCommand, RatingCommandViewModel>
{
    public async Task<Result<RatingCommandViewModel>> Handle(AddRatingCommand request, CancellationToken cancellationToken)
    {
        var rateableAppointment = await rateableAppointmentRepository.GetAsync(request.AppointmentId, cancellationToken);
        if (rateableAppointment == null)
            return Result<RatingCommandViewModel>.Failure(ResponseList.AppointmentNotFound);
        if(rateableAppointment.PatientId != request.UserId)
            return Result<RatingCommandViewModel>.Failure(ResponseList.AppointmentNotYours);
        if(rateableAppointment.IsConsumed)
            return Result<RatingCommandViewModel>.Failure(ResponseList.AlreadyRated);
        
        var rating = Rating.Create(
            rateableAppointment.DoctorId,
            rateableAppointment.PatientId,
            rateableAppointment.Id,
            request.Score,
            request.Comment);
        
        await ratingRepository.AddAsync(rating, cancellationToken);
        
        var doctorRatingStats = await doctorRatingStatsRepository.GetOrCreateByIdAsync(rating.DoctorId, cancellationToken);
        doctorRatingStats.ApplyNewRating(request.Score);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        var ratingCommandViewModel = rating.ToCommandViewModel();
        return Result<RatingCommandViewModel>.Success(ratingCommandViewModel);
    }
}
