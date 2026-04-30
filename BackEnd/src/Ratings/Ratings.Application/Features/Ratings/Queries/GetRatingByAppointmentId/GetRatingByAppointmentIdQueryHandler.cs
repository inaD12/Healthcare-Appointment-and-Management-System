using Ratings.Application.Features.Ratings.Mappers;
using Ratings.Application.Features.Ratings.Models;
using Ratings.Domain.Abstractions.Repositories;
using Ratings.Domain.Utilities;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Ratings.Application.Features.Ratings.Queries.GetRatingByAppointmentId;

public sealed class GetRatingByAppointmentIdQueryHandler(IRatingRepository ratingRepository)
	: IQueryHandler<GetRatingByAppointmentIdQuery, RatingQueryViewModel>
{
	public async Task<Result<RatingQueryViewModel>> Handle(GetRatingByAppointmentIdQuery request, CancellationToken cancellationToken)
	{
		var rating = await ratingRepository.GetByAppointmentId(request.AppointmentId, cancellationToken);
		if (rating == null)
			return Result<RatingQueryViewModel>.Failure(ResponseList.RatingNotFound);

		var ratingQueryViewModel = rating.ToQueryViewModel();
		return Result<RatingQueryViewModel>.Success(ratingQueryViewModel);
	}
}
