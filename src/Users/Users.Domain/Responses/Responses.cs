using Shared.Domain.Results;
using System.Net;
using Users.Domain.Utilities.Strings;

namespace Users.Domain.Responses;

public static class Responses
{
	// Success Responses
	public static Response Ok => Response.Create(SuccessMessages.OperationSuccessful, HttpStatusCode.OK);
	public static Response RegistrationSuccessful => Response.Create(SuccessMessages.RegistrationSuccessful, HttpStatusCode.Created);
	public static Response UpdateSuccessful => Response.Create(SuccessMessages.UpdateSuccessful, HttpStatusCode.OK);

	// Error Responses
	public static Response UserNotFound => Response.Create(ErrorMessages.UserNotFound, HttpStatusCode.NotFound);
	public static Response NoUsersFound => Response.Create(ErrorMessages.NoUsersFound, HttpStatusCode.NotFound);
	public static Response IncorrectPassword => Response.Create(ErrorMessages.IncorrectPassword, HttpStatusCode.Unauthorized);
	public static Response EmailTaken => Response.Create(ErrorMessages.EmailTaken, HttpStatusCode.Conflict);
	public static Response InternalError => Response.Create(ErrorMessages.InternalError, HttpStatusCode.InternalServerError);
	public static Response TokenNotFound => Response.Create(ErrorMessages.TokenNotFound, HttpStatusCode.NotFound);
	public static Response InvalidVerificationToken => Response.Create(ErrorMessages.InvalidVerificationToken, HttpStatusCode.BadRequest);
	public static Response EmailNotSent => Response.Create(ErrorMessages.EmailNotSent, HttpStatusCode.InternalServerError);
}
