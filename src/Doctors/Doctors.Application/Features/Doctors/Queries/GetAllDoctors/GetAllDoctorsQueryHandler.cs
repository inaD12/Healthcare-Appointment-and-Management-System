using Doctors.Application.Features.Doctors.Mappers;
using Doctors.Application.Features.Doctors.Models;
using Doctors.Domain.Infrastructure.Abstractions.Repositories;
using Doctors.Domain.Responses;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Doctors.Application.Features.Doctors.Queries.GetAllDoctors;

public sealed class GetAllDoctorsQueryHandler(
	IDoctorRepository doctorRepository)
	: IQueryHandler<GetAllDoctorsQuery, DoctorPaginatedQueryViewModel>
{

	public async Task<Result<DoctorPaginatedQueryViewModel>> Handle(GetAllDoctorsQuery request, CancellationToken cancellationToken)
	{
		var userPagedListQuery = request.ToInfraQuery();
		var doctorsPagedList = await doctorRepository.GetAllAsync(userPagedListQuery, cancellationToken);
		if (doctorsPagedList == null)
			return Result<DoctorPaginatedQueryViewModel>.Failure(ResponseList.DoctorNotFound);

		var userPaginatedQueryViewModel = doctorsPagedList.ToViewModel();
		return Result<DoctorPaginatedQueryViewModel>.Success(userPaginatedQueryViewModel);
	}
}
