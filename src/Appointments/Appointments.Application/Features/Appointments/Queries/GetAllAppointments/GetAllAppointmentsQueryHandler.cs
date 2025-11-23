using Appointments.Application.Features.Appointments.Mappers;
using Appointments.Application.Features.Appointments.Models;
using Appointments.Domain.Abstractions;
using Appointments.Domain.Utilities;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Appointments.Application.Features.Appointments.Queries.GetAllAppointments;

public class GetAllAppointmentsQueryHandler(IAppointmentRepository appointmentRepository)
	: IQueryHandler<GetAllAppointmentsQuery, AppointmentPaginatedQueryViewModel>
{
	public async Task<Result<AppointmentPaginatedQueryViewModel>> Handle(GetAllAppointmentsQuery request, CancellationToken cancellationToken)
	{
		var appointmentPagedListQuery = request.ToPagedListQuery();
		var appointmentPagedList = await appointmentRepository.GetAllAsync(appointmentPagedListQuery, cancellationToken);
		if (appointmentPagedList == null)
			return Result<AppointmentPaginatedQueryViewModel>.Failure(ResponseList.NoAppointmentsFound);

		var appointmentPaginatedQueryViewModel = appointmentPagedList.ToViewModel();
		return Result<AppointmentPaginatedQueryViewModel>.Success(appointmentPaginatedQueryViewModel);
	}
}
