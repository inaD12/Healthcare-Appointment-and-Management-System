using Appointments.Application.Features.Appointments.Models;
using Appointments.Domain.Enums;
using Shared.Domain.Results;

namespace Appointments.Application.Features.Appointments.Helpers.Abstractions;

public interface IAppointmentService
{
	Task<Result> CreateAppointment(CreateAppointmentModel model);
}