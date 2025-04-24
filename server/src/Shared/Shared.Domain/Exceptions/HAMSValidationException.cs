namespace Shared.Domain.Exceptions;

public class HAMSValidationException : Exception
{
	public string PropertyName { get; }
	public string ErrorMessage { get; }

	public HAMSValidationException(string propertyName, string errorMessage)
		: base($"Validation failed for '{propertyName}': {errorMessage}")
	{
		PropertyName = propertyName;
		ErrorMessage = errorMessage;
	}
}
