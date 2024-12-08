using Appointments.Application.Appoints.Commands.Shared;
using Appointments.Application.Managers.Interfaces;
using Appointments.Domain.Responses;
using Contracts.Abstractions.Messaging;
using Contracts.Results;

namespace Appointments.Application.Appoints.Commands.CreateAppointment;

internal sealed class CreateAppointmentCommandHandler : ICommandHandler<CreateAppointmentCommand>
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

		var patientDataRes = await _repositoryManager.UserData.GetUserDataByEmailAsync(request.PatientEmail);
		if (patientDataRes.IsFailure)
			return Result.Failure(Responses.PatientNotFound);

		var doctorData = doctorDataRes.Value;
		var patientData = patientDataRes.Value;

		var helperResult = await _helper.CreateAppointment(doctorData.UserId, patientData.UserId, request.ScheduledStartTime, request.Duration);

		return helperResult;
	}
}
