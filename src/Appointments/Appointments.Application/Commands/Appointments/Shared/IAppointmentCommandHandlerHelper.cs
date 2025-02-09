using Appointments.Domain.Enums;
using Shared.Domain.Results;

namespace Appointments.Application.Commands.Appointments.Shared;

public interface IAppointmentCommandHandlerHelper
{
	Task<Result> CreateAppointment(string doctorId, string patientId, DateTime startTime, AppointmentDuration duration);
}