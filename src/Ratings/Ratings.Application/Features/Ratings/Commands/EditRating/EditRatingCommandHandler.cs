using Ratings.Domain.Abstractions.Repositories;
using Ratings.Domain.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Ratings.Application.Features.Ratings.Commands.EditRating;

public sealed class EditRatingCommandHandler(
    IRatingRepository ratingRepository,
    IDoctorRatingStatsRepository doctorRatingStatsRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<EditRatingCommand>
{
    public async Task<Result> Handle(EditRatingCommand request, CancellationToken cancellationToken)
    {
        var rating = await ratingRepository.GetByIdAsync(request.RatingId, cancellationToken);
        if (rating == null)
            return Result.Failure(ResponseList.RatingNotFound);
        if(rating.PatientId != request.UserId)
            return Result.Failure(ResponseList.RatingNotYours);

        if (request.Score != null)
        {
            var doctorRatingStats = await doctorRatingStatsRepository.GetByIdAsync(rating.DoctorId, cancellationToken);
            if (doctorRatingStats == null)
            {
                throw new ApplicationException(
                    $"Invariant violated: DoctorRatingStats must exist before editing ratings. DoctorId='{rating.DoctorId}'.");
            }
            
            doctorRatingStats.RemoveRating(rating.Score);
            
            rating.UpdateScore(request.Score.Value);
            doctorRatingStats!.ApplyNewRating(request.Score.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Comment))
        {
            rating.UpdateComment(request.Comment);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}
