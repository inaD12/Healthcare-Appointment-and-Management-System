using Doctors.Application.Features.Doctors.Mappers;
using Doctors.Application.Features.Doctors.Models;
using Doctors.Domain.Abstractions.Repositories;
using Doctors.Domain.Utilities;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Doctors.Application.Features.Doctors.Queries.GetDoctorById;

public sealed class GetDoctorByIdQueryHandler(
	IDoctorRepository doctorRepository)
	: IQueryHandler<GetDoctorByIdQuery, DoctorQueryViewModel>
{
	public async Task<Result<DoctorQueryViewModel>> Handle(GetDoctorByIdQuery request, CancellationToken cancellationToken)
	{
		var doctor = await doctorRepository.GetByIdAsync(request.Id, cancellationToken);
		if (doctor == null)
			return Result<DoctorQueryViewModel>.Failure(ResponseList.DoctorNotFound);

		var doctorQueryViewModel = doctor.ToQueryViewModel();
		return Result<DoctorQueryViewModel>.Success(doctorQueryViewModel);
	}
}
