using Ratings.Application.Features.Ratings.Mappers;
using Ratings.Application.Features.Ratings.Models;
using Ratings.Domain.Abstractions.Repositories;
using Ratings.Domain.Utilities;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Ratings.Application.Features.Ratings.Queries.GetRatingById;

public sealed class GetRatingByIdQueryHandler(IRatingRepository ratingRepository)
	: IQueryHandler<GetRatingByIdQuery, RatingQueryViewModel>
{
	public async Task<Result<RatingQueryViewModel>> Handle(GetRatingByIdQuery request, CancellationToken cancellationToken)
	{
		var rating = await ratingRepository.GetByIdAsync(request.Id, cancellationToken);
		if (rating == null)
			return Result<RatingQueryViewModel>.Failure(ResponseList.RatingNotFound);

		var ratingQueryViewModel = rating.ToQueryViewModel();
		return Result<RatingQueryViewModel>.Success(ratingQueryViewModel);
	}
}
