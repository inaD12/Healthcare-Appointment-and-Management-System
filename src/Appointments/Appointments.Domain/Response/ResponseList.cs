using Appointments.Domain.Strings;
using Shared.Domain.Results;
using System.Net;

namespace Appointments.Domain.Responses;

public static class ResponseList
{
	// Success Responses
	public static Response Ok => Response.Create(SuccessMessages.OperationSuccessful, HttpStatusCode.OK);
	public static Response AppointmentCreated => Response.Create(SuccessMessages.AppointmentCreated, HttpStatusCode.Created);

	// Error Responses
	public static Response AppointmentNotFound => Response.Create(ErrorMessages.AppointmentNotFound, HttpStatusCode.NotFound);
	public static Response DoctorNotFound => Response.Create(ErrorMessages.DoctorNotFound, HttpStatusCode.NotFound);
	public static Response PatientNotFound => Response.Create(ErrorMessages.PatientNotFound, HttpStatusCode.NotFound);
	public static Response EntityNotFound => Response.Create(ErrorMessages.AppointmentNotFound, HttpStatusCode.NotFound);
	public static Response TimeSlotNotAvailable => Response.Create(ErrorMessages.TimeSlotNotAvailable, HttpStatusCode.Conflict);
	public static Response InternalError => Response.Create(ErrorMessages.InternalError, HttpStatusCode.InternalServerError);
	public static Response UserDataNotFound => Response.Create(ErrorMessages.UserDataNotFound, HttpStatusCode.NotFound);
	public static Response CannotCancelOthersAppointment => Response.Create(ErrorMessages.CannotCancelOthersAppointment, HttpStatusCode.Unauthorized);
	public static Response CannotRescheduleOthersAppointment => Response.Create(ErrorMessages.CannotRescheduleOthersAppointment, HttpStatusCode.Unauthorized);
	public static Response UserIsNotADoctor => Response.Create(ErrorMessages.UserIsNotADoctor, HttpStatusCode.Conflict);
	public static Response AppointmentNotScheduled => Response.Create(ErrorMessages.AppointmentNotScheduled, HttpStatusCode.Conflict);
	public static Response AppointmentAlreadyStarted => Response.Create(ErrorMessages.AppointmentAlreadyStarted, HttpStatusCode.Conflict);
}
