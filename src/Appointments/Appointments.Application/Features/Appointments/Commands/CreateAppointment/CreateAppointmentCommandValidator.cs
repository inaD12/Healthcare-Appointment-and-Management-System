using Appointments.Domain.Utilities;
using FluentValidation;

namespace Appointments.Application.Features.Appointments.Commands.CreateAppointment;

public class CreateAppointmentCommandValidator : AbstractValidator<CreateAppointmentCommand>
{
	public CreateAppointmentCommandValidator()
	{
		RuleFor(x => x.PatientUserId)
			.NotEmpty()
			.MinimumLength(AppointmentsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(AppointmentsBusinessConfiguration.ID_MAX_LENGTH);

		RuleFor(x => x.DoctorUserId)
			.NotEmpty()
			.MinimumLength(AppointmentsBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(AppointmentsBusinessConfiguration.ID_MAX_LENGTH);

		RuleFor(x => x.ScheduledStartTime)
				.GreaterThan(DateTime.Now)
				.Must((command, startTime) =>
				{
					var endTime = startTime.AddMinutes((int)command.Duration);
					return startTime < endTime;
				}).WithMessage("Scheduled start time must be before the calculated end time.");

		RuleFor(x => x.Duration)
				.IsInEnum();
	}
}
