using Microsoft.AspNetCore.Http;
using Shared.Domain.Results;
using System.Net;

namespace Shared.API.Helpers;

public static class ControllerResponse
{
	public static IResult ParseAndReturnMessage<T>(Result<T> result)
	{
		if (result.IsSuccess)
		{
			return Results.Json(new { Message = result.Response?.Message.Message, data = result.Value },
								statusCode: (int)(result.Response?.StatusCode ?? HttpStatusCode.OK));
		}

		if (result.IsFailure)
		{
			return Results.Json(new { Message = result.Response.Message.Message },
								statusCode: (int)result.Response.StatusCode);
		}

		return Results.Json(new { Message = "Unknown result state" }, statusCode: 500);
	}

	public static IResult ParseAndReturnMessage(Result result)
	{
		if (result.IsSuccess)
		{
			return Results.Json(new { Message = result.Response?.Message.Message },
								statusCode: (int)(result.Response?.StatusCode ?? HttpStatusCode.OK));
		}

		if (result.IsFailure)
		{
			return Results.Json(new { Message = result.Response.Message.Message },
								statusCode: (int)result.Response.StatusCode);
		}

		return Results.Json(new { Message = "Unknown result state" }, statusCode: 500);
	}

	public static IResult ParseAndReturnMessage<T>(Result<T> result, object returnObject)	
	{
		if (result.IsSuccess)
		{
			return Results.Json(new { Message = result.Response?.Message.Message, data = returnObject },
								statusCode: (int)(result.Response?.StatusCode ?? HttpStatusCode.OK));
		}

		if (result.IsFailure)
		{
			return Results.Json(new { Message = result.Response.Message.Message },
								statusCode: (int)result.Response.StatusCode);
		}

		return Results.Json(new { Message = "Unknown result state" }, statusCode: 500);
	}
}
