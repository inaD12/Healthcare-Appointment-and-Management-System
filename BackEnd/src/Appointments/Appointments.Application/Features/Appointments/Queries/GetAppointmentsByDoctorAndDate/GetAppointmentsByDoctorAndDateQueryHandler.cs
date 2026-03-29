using Appointments.Application.Features.Appointments.Mappers;
using Appointments.Application.Features.Appointments.Models;
using Appointments.Domain.Abstractions;
using Appointments.Domain.Utilities;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Appointments.Application.Features.Appointments.Queries.GetAppointmentsByDoctorAndDate;

public sealed class GetAppointmentsByDoctorAndDateQueryHandler(IAppointmentRepository appointmentRepository)
	: IQueryHandler<GetAppointmentsByDoctorAndDateQuery, ICollection<AppointmentQueryViewModel>>
{
	public async Task<Result<ICollection<AppointmentQueryViewModel>>> Handle(GetAppointmentsByDoctorAndDateQuery request, CancellationToken cancellationToken)
	{
		var appointments = await appointmentRepository.GetByDoctorAndDateAsync(request.DoctorUserId, request.StartDate, request.EndDate, cancellationToken);
		if (appointments.Count == 0)
		{
			return Result<ICollection<AppointmentQueryViewModel>>.Failure(ResponseList.NoAppointmentsFound);
		}

		var bookings = appointments.ToAppointmentsQueryViewModelCollection();
		return Result<ICollection<AppointmentQueryViewModel>>.Success(bookings);
	}
}
