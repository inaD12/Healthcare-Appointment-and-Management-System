using Appointments.Application.Features.Commands.Appointments.Shared;
using Appointments.Application.Features.Jobs.Managers.Interfaces;
using Appointments.Domain.Responses;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Enums;
using Shared.Domain.Results;

namespace Appointments.Application.Features.Commands.Appointments.CreateAppointment;

public sealed class CreateAppointmentCommandHandler : ICommandHandler<CreateAppointmentCommand>
{
	private readonly IRepositoryManager _repositoryManager;
	private readonly IAppointmentCommandHandlerHelper _helper;
	public CreateAppointmentCommandHandler(IRepositoryManager repositoryManager, IAppointmentCommandHandlerHelper helper)
	{
		_repositoryManager = repositoryManager;
		_helper = helper;
	}
	public async Task<Result> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
	{
		var doctorDataRes = await _repositoryManager.UserData.GetUserDataByEmailAsync(request.DoctorEmail);

		if (doctorDataRes.IsFailure)
			return Result.Failure(Responses.DoctorNotFound);
		if (doctorDataRes.Value!.Role != Roles.Doctor)
			return Result.Failure(Responses.UserIsNotADoctor);

		var patientDataRes = await _repositoryManager.UserData.GetUserDataByEmailAsync(request.PatientEmail);

		if (patientDataRes.IsFailure)
			return Result.Failure(Responses.PatientNotFound);

		var doctorData = doctorDataRes.Value;
		var patientData = patientDataRes.Value!;

		var helperResult = await _helper.CreateAppointment(
			doctorData.UserId,
			patientData.UserId,
			request.ScheduledStartTime.ToUniversalTime(),
			request.Duration);

		return helperResult;
	}
}
