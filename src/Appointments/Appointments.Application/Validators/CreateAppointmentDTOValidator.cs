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
			.GreaterThan(DateTime.Now)
			.WithMessage("Scheduled start time must be in the future.")
			.Must((dto, startTime) =>
			{
				var endTime = startTime.AddMinutes((int)dto.Duration);
				return startTime < endTime;
			})
			.WithMessage("Scheduled start time must be before the calculated end time.");

			RuleFor(x => x.Duration)
			.IsInEnum()
			.WithMessage("Invalid appointment duration. Allowed values are: 15, 30, or 60 minutes.");
		}
	}
}
