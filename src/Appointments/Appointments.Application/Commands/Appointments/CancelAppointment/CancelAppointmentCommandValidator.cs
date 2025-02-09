using Appointments.Domain.Utilities;
using FluentValidation;

namespace Appointments.Application.Commands.Appointments.CancelAppointment;

public class CancelAppointmentCommandValidator : AbstractValidator<CancelAppointmentCommand>
{
	public CancelAppointmentCommandValidator()
	{
		RuleFor(x => x.AppointmentId)
			.NotEmpty()
			.MinimumLength(AppointmentsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(AppointmentsBusinessConfiguration.ID_MAX_LENGTH);
	}
}
