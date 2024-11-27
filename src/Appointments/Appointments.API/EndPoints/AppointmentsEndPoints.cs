using Appointments.API.Extentions;
using Appointments.Application.Services;
using Appointments.Domain.DTOS.Request;
using Appointments.Domain.DTOS.Response;
using FluentValidation;

namespace Appointments.API.EndPoints
{
	internal class AppointmentsEndPoints : IEndPoints
	{
		public void RegisterEndpoints(IEndpointRouteBuilder app)
		{
			var group = app.MapGroup("api/appointments");

			group.MapPut("create", Create)
				.Produces<MessageDTO>(StatusCodes.Status200OK)
				.Produces(StatusCodes.Status400BadRequest)
				.Produces(StatusCodes.Status401Unauthorized)
				.Produces<MessageDTO>(StatusCodes.Status404NotFound)
				.Produces<MessageDTO>(StatusCodes.Status409Conflict)
				.Produces(StatusCodes.Status500InternalServerError);
				//.RequireAuthorization();
		}

		private async Task<IResult?> ValidateAndReturnResponse<T>(T dto, IValidator<T> validator)
		{
			var valResult = await validator.ValidateAsync(dto);

			if (!valResult.IsValid)
			{
				var errors = valResult.Errors.Select(e => e.ErrorMessage).ToList();
				return Results.BadRequest(new { Errors = errors });
			}

			return null;
		}
		public async Task<IResult> Create(
			CreateAppointmentDTO appointmentDTO,
			IAppointentService appointentService,
			IValidator<CreateAppointmentDTO> validator)
		{
			var validationResponse = await ValidateAndReturnResponse(appointmentDTO, validator);
			if (validationResponse != null)
				return validationResponse;

			var res = await appointentService.CreateAsync(appointmentDTO);

			return ControllerResponse.ParseAndReturnMessage(res);
		}
	}
}
