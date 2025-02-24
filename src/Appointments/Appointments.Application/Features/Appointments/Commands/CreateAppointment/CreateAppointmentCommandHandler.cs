using Appointments.Application.Features.Appointments.Helpers.Abstractions;
using Appointments.Application.Features.Appointments.Models;
using Appointments.Application.Features.Jobs.Managers.Interfaces;
using Appointments.Domain.Responses;
using Shared.Application.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Enums;
using Shared.Domain.Results;

namespace Appointments.Application.Features.Commands.Appointments.CreateAppointment;

public sealed class CreateAppointmentCommandHandler : ICommandHandler<CreateAppointmentCommand>
{
	private readonly IRepositoryManager _repositoryManager;
	private readonly IAppointmentService _appointmentService;
	private readonly IHAMSMapper _mapper;
	public CreateAppointmentCommandHandler(IRepositoryManager repositoryManager, IAppointmentService appointmentServuce, IHAMSMapper mapper)
	{
		_repositoryManager = repositoryManager;
		_appointmentService = appointmentServuce;
		_mapper = mapper;
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

		var doctorPatientIdModel = new DoctorPatientIdModel(doctorDataRes.Value.UserId, patientDataRes.Value!.UserId);
		var createAppointmentModel = _mapper.Map<CreateAppointmentModel>((doctorPatientIdModel, request));
		var helperResult = await _appointmentService.CreateAppointment(createAppointmentModel);

		return helperResult;
	}
}
