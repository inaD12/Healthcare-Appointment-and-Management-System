using Appointments.Domain.DTOS.Request;
using FluentValidation;

namespace Appointments.Application.Validators
{
	public class CreateAppointmentDTOValidator : AbstractValidator<CreateAppointmentDTO>
	{
		public CreateAppointmentDTOValidator()
		{
			RuleFor(x => x.PatientEmail)
				.NotEmpty().WithMessage("Patient email is required.")
				.EmailAddress().WithMessage("Patient email must be a valid email address.");

			RuleFor(x => x.DoctorEmail)
				.NotEmpty().WithMessage("Doctor email is required.")
				.EmailAddress().WithMessage("Doctor email must be a valid email address.");

			RuleFor(x => x.ScheduledStartTime)
				.GreaterThan(DateTime.Now).WithMessage("Scheduled start time must be in the future.")
				.LessThan(x => x.ScheduledEndTime).WithMessage("Scheduled start time must be before the end time.");

			RuleFor(x => x.ScheduledEndTime)
				.GreaterThan(x => x.ScheduledStartTime).WithMessage("Scheduled end time must be after the start time.");
		}
	}
}
