namespace Users.Domain.Utilities.Strings;

public static class ErrorMessages
{
	public const string UserNotFound = "User not found";
	public const string NoUsersFound = "No users in the system";
	public const string IncorrectPassword = "Incorrect password";
	public const string EmailTaken = "This email has been taken";
	public const string InternalError = "Internal error, please try again";

	public const string TokenNotFound = "Token not found";
	public const string InvalidVerificationToken = "Invalid verification token";
	public const string EmailNotSent = "Email wasn't sent due to an internal error";
}
