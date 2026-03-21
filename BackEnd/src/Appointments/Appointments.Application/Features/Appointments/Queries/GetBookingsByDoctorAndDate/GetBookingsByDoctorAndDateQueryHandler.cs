using Appointments.Application.Features.Appointments.Mappers;
using Appointments.Application.Features.Appointments.Models;
using Appointments.Domain.Abstractions;
using Appointments.Domain.Utilities;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;

namespace Appointments.Application.Features.Appointments.Queries.GetBookingsByDoctorAndDate;

public sealed class GetBookingsByDoctorAndDateQueryHandler(IAppointmentRepository appointmentRepository)
	: IQueryHandler<GetBookingsByDoctorAndDateQuery, ICollection<BookingQueryViewModel>>
{
	public async Task<Result<ICollection<BookingQueryViewModel>>> Handle(GetBookingsByDoctorAndDateQuery request, CancellationToken cancellationToken)
	{
		var appointments = await appointmentRepository.GetByDoctorAndDateAsync(request.DoctorUserId, request.StartDate, request.EndDate, cancellationToken);
		if (appointments.Count == 0)
		{
			return Result<ICollection<BookingQueryViewModel>>.Failure(ResponseList.NoAppointmentsFound);
		}

		var bookings = appointments.ToBookingQueryViewModelCollection();
		return Result<ICollection<BookingQueryViewModel>>.Success(bookings);
	}
}
