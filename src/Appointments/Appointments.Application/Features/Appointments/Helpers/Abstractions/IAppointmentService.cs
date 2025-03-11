using Appointments.Application.Features.Appointments.Models;
using Shared.Domain.Results;

namespace Appointments.Application.Features.Appointments.Helpers.Abstractions;

public interface IAppointmentService
{
	Task<Result<AppointmentCommandViewModel>> CreateAppointment(CreateAppointmentModel model);
}