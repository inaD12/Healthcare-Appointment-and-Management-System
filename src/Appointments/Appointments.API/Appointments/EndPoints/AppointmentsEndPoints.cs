using Appointments.API.Appointments.Models.Requests;
using Appointments.Application.Features.Appointments.Models;
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
			.Produces<AppointmentCommandViewModel>(StatusCodes.Status201Created)
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
			.Produces<AppointmentCommandViewModel>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError);
		//.RequireAuthorization();

		group.MapGet("getById/{id}", GetById)
			.Produces<AppointmentQueryViewModel>(StatusCodes.Status200OK)
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
		return ControllerResponse.ParseAndReturnMessage(res);
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
		return ControllerResponse.ParseAndReturnMessage(res);
	}

	public async Task<IResult> GetById(
		[FromRoute] string id,
		[FromServices] ISender sender,
		[FromServices] IHAMSMapper mapper,
		CancellationToken cancellationToken)
	{
		var query = mapper.Map<GetAppointmentByIdQuery>(id);
		var res = await sender.Send(query, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}
}
