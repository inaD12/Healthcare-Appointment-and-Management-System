using Appointments.API.Appointments.Models.Requests;
using Appointments.API.Appointments.Models.Responses;
using Appointments.Application.Features.Appointments.Queries.GetAllAppointments;
using Appointments.Application.Features.Commands.Appointments.CancelAppointment;
using Appointments.Application.Features.Commands.Appointments.CreateAppointment;
using Appointments.Application.Features.Commands.Appointments.RescheduleAppointment;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.API.Abstractions;
using Shared.API.Helpers;
using Shared.Application.Abstractions;

namespace Appointments.API.EndPoints;

internal class AppointmentsEndPoints : IEndPoints
{
	public void RegisterEndpoints(IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("api/appointments");

		group.MapPost("create", Create)
			.Produces<AppointmentCommandResponse>(StatusCodes.Status201Created)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError);
		//.RequireAuthorization();

		group.MapPut("cancel", Cancel)
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError);
		//.RequireAuthorization();

		group.MapPut("reschedule", Reschedule)
			.Produces<AppointmentCommandResponse>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError);
		//.RequireAuthorization();

		group.MapGet("getById/{id}", GetById)
			.Produces<AppointmentQueryResponse>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status500InternalServerError);
		//.RequireAuthorization();
	}

	public async Task<IResult> Create(
		[FromBody] CreateAppointmentRequest request,
		[FromServices] ISender sender,
		[FromServices] IHAMSMapper mapper,
		CancellationToken cancellationToken)
	{
		var command = mapper.Map<CreateAppointmentCommand>(request);
		var res = await sender.Send(command, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		var appointmentCommandResponse = mapper.Map<AppointmentCommandResponse>(res.Value!);
		return ControllerResponse.ParseAndReturnMessage(res, appointmentCommandResponse);
	}

	public async Task<IResult> Cancel(
		[FromBody] CancelAppointmentRequest request,
		[FromServices] ISender sender,
		[FromServices] IHAMSMapper mapper,
		CancellationToken cancellationToken)
	{
		var command = mapper.Map<CancelAppointmentCommand>(request);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}

	public async Task<IResult> Reschedule(
		[FromBody] RescheduleAppointmentRequest request,
		[FromServices] ISender sender,
		[FromServices] IHAMSMapper mapper,
		CancellationToken cancellationToken)
	{
		var command = mapper.Map<RescheduleAppointmentCommand>(request);
		var res = await sender.Send(command, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		var appointmentCommandResponse = mapper.Map<AppointmentCommandResponse>(res.Value!);
		return ControllerResponse.ParseAndReturnMessage(res, appointmentCommandResponse);
	}

	public async Task<IResult> GetById(
		[FromRoute] string id,
		[FromServices] ISender sender,
		[FromServices] IHAMSMapper mapper,
		CancellationToken cancellationToken)
	{
		var query = mapper.Map<GetAppointmentByIdQuery>(id);
		var res = await sender.Send(query, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		var appointmentCommandResponse = mapper.Map<AppointmentQueryResponse>(res.Value!);
		return ControllerResponse.ParseAndReturnMessage(res, appointmentCommandResponse);
	}
}
