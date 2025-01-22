﻿using System.Net;

namespace Contracts.Results
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
		public static Response Create(string message, HttpStatusCode statusCode) => new Response(message, statusCode);

		public static Response Ok => Response.Create("Operation successful", HttpStatusCode.OK);
	}
}
