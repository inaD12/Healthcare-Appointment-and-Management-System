using System.Net;
using Ratings.Domain.Utilities.Strings;
using Shared.Domain.Results;

namespace Ratings.Domain.Utilities;

public static class ResponseList
{
	// Success Responses

	// Error Responses
	public static Response AppointmentNotFound => Response.Create(ErrorMessages.AppointmentNotFound, HttpStatusCode.NotFound);
	public static Response AppointmentNotYours => Response.Create(ErrorMessages.AppointmentNotYours, HttpStatusCode.Forbidden);
	public static Response AlreadyRated => Response.Create(ErrorMessages.AlreadyRated, HttpStatusCode.Conflict);
	public static Response RatingNotFound => Response.Create(ErrorMessages.RatingNotFound, HttpStatusCode.NotFound);
	public static Response NoRatingsFound => Response.Create(ErrorMessages.NoRatingsFound, HttpStatusCode.NotFound);
	public static Response DoctorRatingStatsNotFound => Response.Create(ErrorMessages.DoctorRatingStatsNotFound, HttpStatusCode.NotFound);
	public static Response RatingNotYours => Response.Create(ErrorMessages.RatingNotYours, HttpStatusCode.Forbidden);
}
