using System.Security.Claims;
using Appointments.Application.Features.Appointments.Requirements.ModifyAppointment;
using Appointments.Domain.Infrastructure.Abstractions.Repository;
using Appointments.Domain.Responses;
using Microsoft.AspNetCore.Authorization;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Shared.Infrastructure.Clock;

namespace Appointments.Application.Features.Appointments.Commands.CancelAppointment;

public sealed class CancelAppointmentCommandHandler : ICommandHandler<CancelAppointmentCommand>
{
	private readonly IAppointmentRepository _appointmentRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IDateTimeProvider _dateTimeProvider;
	private readonly IAuthorizationService _authService;
	public CancelAppointmentCommandHandler(IUnitOfWork unitOfWork, IDateTimeProvider dateTimeProvider, IAppointmentRepository repositoryManager, IAuthorizationService authService)
	{
		_unitOfWork = unitOfWork;
		_dateTimeProvider = dateTimeProvider;
		_appointmentRepository = repositoryManager;
		_authService = authService;
	}
	public async Task<Result> Handle(CancelAppointmentCommand request, CancellationToken cancellationToken)
	{
		var appointment = await _appointmentRepository.GetByIdAsync(request.AppointmentId);
		if (appointment == null)
		{
			return Result.Failure(ResponseList.AppointmentNotFound);
		}

		var requirement = new ModifyAppointmentRequirement();

		var authResult = await _authService.AuthorizeAsync(ClaimsPrincipal.Current!, appointment, requirement );
		if (!authResult.Succeeded)
		{
			return Result.Failure(ResponseList.CannotCancelOthersAppointment);
		}

		var res = appointment.Cancel(_dateTimeProvider.UtcNow);
		if (res.IsFailure)
		{
			return res;
		}

		await _unitOfWork.SaveChangesAsync(cancellationToken);
		return Result.Success();
	}
}
