using System.Net;
using Doctors.Domain.Utilities.Strings;
using Shared.Domain.Results;

namespace Doctors.Domain.Responses;

public static class ResponseList
{
	// Success Responses

	// Error Responses
	public static Response ExceptionOverlap => Response.Create(ErrorMessages.ExceptionOverlap, HttpStatusCode.Conflict);
	public static Response ExceptionNotFound => Response.Create(ErrorMessages.ExceptionNotFound, HttpStatusCode.NotFound);
	public static Response DuplicateWorkDay => Response.Create(ErrorMessages.DuplicateWorkDay, HttpStatusCode.Conflict);
	public static Response WorkDayNotExist => Response.Create(ErrorMessages.WorkDayNotExist, HttpStatusCode.Conflict);
	public static Response InvalidTimezone => Response.Create(ErrorMessages.InvalidTimezone, HttpStatusCode.BadRequest);
	public static Response DoctorNotFound => Response.Create(ErrorMessages.DoctorNotFound, HttpStatusCode.NotFound);
	public static Response DoctorAlreadyExists => Response.Create(ErrorMessages.DoctorAlreadyExists, HttpStatusCode.Conflict);
}
