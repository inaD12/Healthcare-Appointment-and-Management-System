using Appointments.Domain.DTOS.Request;
using FluentValidation;

namespace Appointments.Application.Validators
{
	public class RescheduleAppointmentDTOValidator : AbstractValidator<RescheduleAppointmentDTO>
	{
		public RescheduleAppointmentDTOValidator()
		{
			RuleFor(x => x.ScheduledStartTime)
				.GreaterThan(DateTime.UtcNow).WithMessage("Scheduled start time must be in the future.");

			RuleFor(x => x.Duration)
				.NotNull().WithMessage("Duration is required.")
				.IsInEnum().WithMessage("Invalid duration value.");
		}
	}
}
