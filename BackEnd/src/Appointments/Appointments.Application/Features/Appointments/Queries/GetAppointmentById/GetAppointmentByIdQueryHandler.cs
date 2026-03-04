using Appointments.Application.Features.Appointments.Mappers;
using Appointments.Application.Features.Appointments.Models;
using Appointments.Domain.Abstractions;
using Appointments.Domain.Utilities;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Appointments.Application.Features.Appointments.Queries.GetAppointmentById;

public sealed class GetAppointmentByIdQueryHandler(IAppointmentRepository appointmentRepository)
	: IQueryHandler<GetAppointmentByIdQuery, AppointmentQueryViewModel>
{
	public async Task<Result<AppointmentQueryViewModel>> Handle(GetAppointmentByIdQuery request, CancellationToken cancellationToken)
	{
		var appointment = await appointmentRepository.GetByIdAsync(request.Id, cancellationToken);
		if (appointment == null)
			return Result<AppointmentQueryViewModel>.Failure(ResponseList.AppointmentNotFound);

		var appointmentQueryViewModel = appointment.ToQueryViewModel();
		return Result<AppointmentQueryViewModel>.Success(appointmentQueryViewModel);
	}
}
