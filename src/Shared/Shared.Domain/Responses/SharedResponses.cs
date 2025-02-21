using Shared.Domain.Results;
using Shared.Domain.Strings;
using System.Net;

namespace Shared.Domain.Responses;

public static class SharedResponses
{
	public static Response ValidationError => Response.Create(ErrorMessages.ValidationError, HttpStatusCode.BadRequest);
	public static Response EntityNotFound => Response.Create(ErrorMessages.EntityNotFound, HttpStatusCode.NotFound);
	public static Response JWTNotFound => Response.Create(ErrorMessages.JWTokenNotFound, HttpStatusCode.NotFound);
	public static Response InternalError => Response.Create(ErrorMessages.InternalError, HttpStatusCode.InternalServerError);
}
