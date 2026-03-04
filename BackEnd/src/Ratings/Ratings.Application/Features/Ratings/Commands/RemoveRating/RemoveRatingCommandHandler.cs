using Ratings.Domain.Abstractions.Repositories;
using Ratings.Domain.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Ratings.Application.Features.Ratings.Commands.RemoveRating;

public sealed class RemoveRatingCommandHandler(
    IRatingRepository ratingRepository,
    IDoctorRatingStatsRepository doctorRatingStatsRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RemoveRatingCommand>
{
    public async Task<Result> Handle(RemoveRatingCommand request, CancellationToken cancellationToken)
    {
        var rating = await ratingRepository.GetByIdAsync(request.RatingId, cancellationToken);
        if (rating == null)
            return Result.Failure(ResponseList.RatingNotFound);
        if(rating.PatientId != request.UserId)
            return Result.Failure(ResponseList.RatingNotYours);
        
        ratingRepository.Delete(rating);
        
        var doctorRatingStats = await doctorRatingStatsRepository.GetByIdAsync(rating.DoctorId, cancellationToken);
        if (doctorRatingStats == null)
        {
            throw new ApplicationException(
                $"Invariant violated: DoctorRatingStats must exist before editing ratings. DoctorId='{rating.DoctorId}'.");
        }
        
        doctorRatingStats.RemoveRating(rating.Score);
        
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
