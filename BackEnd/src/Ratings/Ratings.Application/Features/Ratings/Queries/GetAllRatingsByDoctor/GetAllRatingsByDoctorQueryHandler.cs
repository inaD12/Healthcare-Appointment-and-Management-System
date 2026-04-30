using Ratings.Application.Features.Ratings.Mappers;
using Ratings.Application.Features.Ratings.Models;
using Ratings.Domain.Abstractions.Repositories;
using Ratings.Domain.Utilities;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Ratings.Application.Features.Ratings.Queries.GetAllRatingsByDoctor;

public class GetAllRatingsByDoctorQueryHandler(IRatingRepository ratingRepository)
	: IQueryHandler<GetAllRatingsByDoctorQuery, RatingPaginatedQueryViewModel>
{
	public async Task<Result<RatingPaginatedQueryViewModel>> Handle(GetAllRatingsByDoctorQuery request, CancellationToken cancellationToken)
	{
		var appointmentPagedListQuery = request.ToInfraQuery();
		var appointmentPagedList = await ratingRepository.GetAllAsync(appointmentPagedListQuery, cancellationToken);
		if (appointmentPagedList == null || !appointmentPagedList.Items.Any())
			return Result<RatingPaginatedQueryViewModel>.Failure(ResponseList.NoRatingsFound);

		var appointmentPaginatedQueryViewModel = appointmentPagedList.ToViewModel();
		return Result<RatingPaginatedQueryViewModel>.Success(appointmentPaginatedQueryViewModel);
	}
}
