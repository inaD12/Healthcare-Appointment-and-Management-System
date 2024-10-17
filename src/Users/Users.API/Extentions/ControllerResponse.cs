﻿using Microsoft.AspNetCore.Mvc;
using Users.Domain.Enums;
using Users.Domain.Result;

namespace Healthcare_Appointment_and_Management_System.Extentions
{
	public static class ControllerResponse
	{
		public static IResult ParseAndReturnMessage<T>(Result<T> result)
		{
			if (result.IsSuccess)
			{
				return Results.Json(new { message = result.Response?.Message, data = result.Value },
									statusCode: (int)(result.Response?.StatusCode ?? HttpStatusCode.OK));
			}

			if (result.IsFailure)
			{
				return Results.Json(new { message = result.Response.Message },
									statusCode: (int)result.Response.StatusCode);
			}

			return Results.Json(new { message = "Unknown result state" }, statusCode: 500);
		}

		public static IResult ParseAndReturnMessage(Result result)
		{
			if (result.IsSuccess)
			{
				return Results.Json(new { message = result.Response?.Message, data = result.Object },
									statusCode: (int)(result.Response?.StatusCode ?? HttpStatusCode.OK));
			}

			if (result.IsFailure)
			{
				return Results.Json(new { message = result.Response.Message },
									statusCode: (int)result.Response.StatusCode);
			}

			return Results.Json(new { message = "Unknown result state" }, statusCode: 500);
		}
	}

}