using Appointments.Domain.Utilities;
using FluentValidation;

namespace Appointments.Application.Features.Commands.Appointments.CreateAppointment;

public class CreateAppointmentCommandValidator : AbstractValidator<CreateAppointmentCommand>
{
	public CreateAppointmentCommandValidator()
	{
		RuleFor(x => x.PatientEmail)
				.NotEmpty()
				.MinimumLength(AppointmentsBusinessConfiguration.EMAIL_MIN_LENGTH)
				.MaximumLength(AppointmentsBusinessConfiguration.EMAIL_MAX_LENGTH)
				.EmailAddress();

		RuleFor(x => x.DoctorEmail)
				.NotEmpty()
				.MinimumLength(AppointmentsBusinessConfiguration.EMAIL_MIN_LENGTH)
				.MaximumLength(AppointmentsBusinessConfiguration.EMAIL_MAX_LENGTH)
				.EmailAddress();

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
