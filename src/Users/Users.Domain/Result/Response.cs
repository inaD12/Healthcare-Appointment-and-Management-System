using Users.Domain.Enums;

namespace Users.Domain.Result
{
	public class Response
	{
		public string Message { get; }
		public HttpStatusCode StatusCode { get; }

		private Response(string message, HttpStatusCode statusCode)
		{
			Message = message;
			StatusCode = statusCode;
		}

		public static Response Ok => new("Operation successful", HttpStatusCode.OK);
		public static Response UserNotFound => new("User not found", HttpStatusCode.NotFound);
		public static Response NoUsersFound => new("No users in the system", HttpStatusCode.NotFound);
		public static Response IncorrectPassword => new("Incorrect password", HttpStatusCode.Unauthorized);
		public static Response RegistrationSuccessful => new("Registration successful", HttpStatusCode.Created);
		public static Response EmailTaken => new("This email has been taken", HttpStatusCode.Conflict);
		public static Response UpdateSuccessful => new("Update successful", HttpStatusCode.OK);

	}
}
