using Contracts.Results;
using Shared.Strings;
using System.Net;

namespace Shared.Responses
{
	public static class SharedResponses
	{
		public static Response ValidationError => Response.Create(ErrorMessages.ValidationError, HttpStatusCode.BadRequest);
	}
}
