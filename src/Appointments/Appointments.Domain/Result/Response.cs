﻿using Appointments.Domain.DTOS.Response;
using Appointments.Domain.Strings;
using System.Net;

namespace Appointments.Domain.Result
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

		// Error Responses
		public static Response AppointmentNotFound => Create(ErrorMessages.AppointmentNotFound, HttpStatusCode.NotFound);
		public static Response EntityNotFound => Create(ErrorMessages.AppointmentNotFound, HttpStatusCode.NotFound);
	}
}
