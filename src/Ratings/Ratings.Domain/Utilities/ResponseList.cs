using System.Net;
using Ratings.Domain.Utilities.Strings;
using Shared.Domain.Results;

namespace Ratings.Domain.Utilities;

public static class ResponseList
{
	// Success Responses

	// Error Responses
	public static Response InvalidRatingScore => Response.Create(ErrorMessages.InvalidRatingScore, HttpStatusCode.BadRequest);
	public static Response AppointmentNotFound => Response.Create(ErrorMessages.AppointmentNotFound, HttpStatusCode.NotFound);
	public static Response AppointmentNotYours => Response.Create(ErrorMessages.AppointmentNotYours, HttpStatusCode.Forbidden);
	public static Response AlreadyRated => Response.Create(ErrorMessages.AlreadyRated, HttpStatusCode.Conflict);
}
