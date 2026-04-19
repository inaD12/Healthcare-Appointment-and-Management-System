using Doctors.Application.Features.Specialities.Mappers;
using Doctors.Application.Features.Specialities.Models;
using Doctors.Domain.Abstractions.Repositories;
using Doctors.Domain.Utilities;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Doctors.Application.Features.Specialities.Queries.GetAllDoctors;

public sealed class GetAllSpecialitiesQueryHandler(
	ISpecialityRepository specialityRepository)
	: IQueryHandler<GetAllSpecialitiesQuery, SpecialityPaginatedQueryViewModel>
{

	public async Task<Result<SpecialityPaginatedQueryViewModel>> Handle(GetAllSpecialitiesQuery request, CancellationToken cancellationToken)
	{
		var specialityPagedListQuery = request.ToInfraQuery();
		var specialityPagedList = await specialityRepository.GetAllAsync(specialityPagedListQuery, cancellationToken);
		if (specialityPagedList == null)
			return Result<SpecialityPaginatedQueryViewModel>.Failure(ResponseList.NoSpecialitiesFound);

		var userPaginatedQueryViewModel = specialityPagedList.ToViewModel();
		return Result<SpecialityPaginatedQueryViewModel>.Success(userPaginatedQueryViewModel);
	}
}
