using Appointments.Domain.Utilities;
using FluentValidation;

namespace Appointments.Application.Commands.Appointments.RescheduleAppointment;

public class RescheduleAppointmentCommandValidator : AbstractValidator<RescheduleAppointmentCommand>
{
	public RescheduleAppointmentCommandValidator()
	{
		RuleFor(x => x.AppointmentID)
			.NotEmpty()
			.MinimumLength(AppointmentsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(AppointmentsBusinessConfiguration.ID_MAX_LENGTH);

		RuleFor(x => x.ScheduledStartTime)
				.GreaterThan(DateTime.Now)
				.Must((dto, startTime) =>
				{
					var endTime = startTime.AddMinutes((int)dto.Duration);
					return startTime < endTime;
				}).WithMessage("Scheduled start time must be before the calculated end time.");

		RuleFor(x => x.Duration)
				.NotEmpty()
				.IsInEnum();
	}
}
