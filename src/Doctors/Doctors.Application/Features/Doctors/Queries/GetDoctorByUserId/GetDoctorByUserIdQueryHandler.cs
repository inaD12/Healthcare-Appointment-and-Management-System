using Doctors.Application.Features.Doctors.Mappers;
using Doctors.Application.Features.Doctors.Models;
using Doctors.Application.Features.Doctors.Queries.GetDoctorById;
using Doctors.Domain.Infrastructure.Abstractions.Repositories;
using Doctors.Domain.Responses;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Doctors.Application.Features.Doctors.Queries.GetDoctorByUserId;

public sealed class GetDoctorByUserIdQueryHandler(
	IDoctorRepository doctorRepository)
	: IQueryHandler<GetDoctorByUserIdQuery, DoctorQueryViewModel>
{
	public async Task<Result<DoctorQueryViewModel>> Handle(GetDoctorByUserIdQuery request, CancellationToken cancellationToken)
	{
		var doctor = await doctorRepository.GetByUserIdAsync(request.Id, cancellationToken);
		if (doctor == null)
			return Result<DoctorQueryViewModel>.Failure(ResponseList.DoctorNotFound);

		var doctorQueryViewModel = doctor.ToQueryViewModel();
		return Result<DoctorQueryViewModel>.Success(doctorQueryViewModel);
	}
}
