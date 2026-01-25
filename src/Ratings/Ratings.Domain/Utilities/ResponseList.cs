using System.Net;
using Ratings.Domain.Utilities.Strings;
using Shared.Domain.Results;

namespace Ratings.Domain.Utilities;

public static class ResponseList
{
	// Success Responses

	// Error Responses
	public static Response InvalidRatingScore => Response.Create(ErrorMessages.InvalidRatingScore, HttpStatusCode.BadRequest);
}
