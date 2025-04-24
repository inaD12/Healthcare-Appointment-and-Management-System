using Appointments.Application.Features.Appointments.Queries.GetAllAppointments;
using Appointments.Domain.Utilities;
using FluentValidation;

namespace Appointments.Application.Features.Appointments.Queries.GetAllAppointmentById;

public class GetAppointmentByIdQueryValidator : AbstractValidator<GetAppointmentByIdQuery>
{
	public GetAppointmentByIdQueryValidator()
	{
		RuleFor(q => q.Id)
		  .NotEmpty()
		  .MinimumLength(AppointmentsBusinessConfiguration.ID_MIN_LENGTH)
		  .MaximumLength(AppointmentsBusinessConfiguration.ID_MAX_LENGTH);
	}
}
