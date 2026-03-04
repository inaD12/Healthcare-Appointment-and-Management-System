using System.Net;

namespace Shared.Domain.Results;

public class Response
{
	public MessageDTO Message { get; }
	public HttpStatusCode StatusCode { get; }

	private Response(string message, HttpStatusCode statusCode)
	{
		Message = new MessageDTO(message);
		StatusCode = statusCode;
	}
	public static Response Create(string message, HttpStatusCode statusCode) => new Response(message, statusCode);
	public static Response Create(string messageTemplate, HttpStatusCode statusCode, params object[] args)
		=> new Response(string.Format(messageTemplate, args), statusCode);
	
	public static Response Ok => Create("Operation successful", HttpStatusCode.OK);
}
