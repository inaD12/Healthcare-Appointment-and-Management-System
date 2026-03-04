using Ratings.Application.Features.Ratings.Mappers;
using Ratings.Application.Features.Ratings.Models;
using Ratings.Domain.Abstractions.Repositories;
using Ratings.Domain.Utilities;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Ratings.Application.Features.Ratings.Queries.GetDoctorRatingStatsById;

public sealed class GetDoctorRatingStatsByIdQueryHandler(IDoctorRatingStatsRepository ratingStatsRepository)
	: IQueryHandler<GetDoctorRatingStatsByIdQuery, DoctorRatingStatsQueryViewModel>
{
	public async Task<Result<DoctorRatingStatsQueryViewModel>> Handle(GetDoctorRatingStatsByIdQuery request, CancellationToken cancellationToken)
	{
		var ratingStats = await ratingStatsRepository.GetByIdAsync(request.Id, cancellationToken);
		if (ratingStats == null)
			return Result<DoctorRatingStatsQueryViewModel>.Failure(ResponseList.DoctorRatingStatsNotFound);

		var doctorRatingStatsQueryViewModel = ratingStats.ToQueryViewModel();
		return Result<DoctorRatingStatsQueryViewModel>.Success(doctorRatingStatsQueryViewModel);
	}
}
