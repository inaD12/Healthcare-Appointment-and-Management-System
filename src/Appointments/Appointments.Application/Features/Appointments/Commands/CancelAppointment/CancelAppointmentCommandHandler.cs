using System.Security.Claims;
using Appointments.Application.Features.Appointments.Requirements.ModifyAppointment;
using Appointments.Domain.Abstractions;
using Appointments.Domain.Utilities;
using Microsoft.AspNetCore.Authorization;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Shared.Infrastructure.Clock;

namespace Appointments.Application.Features.Appointments.Commands.CancelAppointment;

public sealed class CancelAppointmentCommandHandler(
	IUnitOfWork unitOfWork,
	IDateTimeProvider dateTimeProvider,
	IAppointmentRepository repositoryManager,
	IAuthorizationService authService)
	: ICommandHandler<CancelAppointmentCommand>
{
	public async Task<Result> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
	{
		var appointment = await repositoryManager.GetByIdAsync(request.AppointmentId, cancellationToken);
		if (appointment == null)
		{
			return Result.Failure(ResponseList.AppointmentNotFound);
		}

		var requirement = new ModifyAppointmentRequirement();

		var authResult = await authService.AuthorizeAsync(ClaimsPrincipal.Current!, appointment, requirement );
		if (!authResult.Succeeded)
		{
			return Result.Failure(ResponseList.CannotCancelOthersAppointment);
		}

		var res = appointment.Cancel(dateTimeProvider.UtcNow);
		if (res.IsFailure)
		{
			return res;
		}

		await unitOfWork.SaveChangesAsync(cancellationToken);
		return Result.Success();
	}
}
