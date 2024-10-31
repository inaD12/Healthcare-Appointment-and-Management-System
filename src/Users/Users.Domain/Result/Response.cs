using Users.Domain.DTOs.Responses;
using Users.Domain.Enums;
using Users.Domain.Strings;

namespace Users.Domain.Result
{
    public class Response
	{
		public MessageDTO Message { get; }
		public HttpStatusCode StatusCode { get; }

		private Response(string message, HttpStatusCode statusCode)
		{
			Message = new MessageDTO(message);
			StatusCode = statusCode;
		}
		private static Response Create(string message, HttpStatusCode statusCode) => new Response(message, statusCode);

		// Success Responses
		public static Response Ok => Create(SuccessMessages.OperationSuccessful, HttpStatusCode.OK);
		public static Response RegistrationSuccessful => Create(SuccessMessages.RegistrationSuccessful, HttpStatusCode.Created);
		public static Response UpdateSuccessful => Create(SuccessMessages.UpdateSuccessful, HttpStatusCode.OK);

		// Error Responses
		public static Response UserNotFound => Create(ErrorMessages.UserNotFound, HttpStatusCode.NotFound);
		public static Response NoUsersFound => Create(ErrorMessages.NoUsersFound, HttpStatusCode.NotFound);
		public static Response IncorrectPassword => Create(ErrorMessages.IncorrectPassword, HttpStatusCode.Unauthorized);
		public static Response EmailTaken => Create(ErrorMessages.EmailTaken, HttpStatusCode.Conflict);
		public static Response InternalError => Create(ErrorMessages.InternalError, HttpStatusCode.InternalServerError);

		public static Response TokenNotFound => Create(ErrorMessages.TokenNotFound, HttpStatusCode.NotFound);
		public static Response InvalidVerificationToken => Create(ErrorMessages.InvalidVerificationToken, HttpStatusCode.BadRequest);
		public static Response EmailNotSent => Create(ErrorMessages.EmailNotSent, HttpStatusCode.InternalServerError);

	}
}
