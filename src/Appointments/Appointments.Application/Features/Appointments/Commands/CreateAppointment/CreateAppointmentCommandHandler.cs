using Appointments.Application.Features.Appointments.Helpers.Abstractions;
using Appointments.Application.Features.Appointments.Models;
using Appointments.Application.Features.Jobs.Managers.Interfaces;
using Appointments.Domain.Responses;
using Shared.Application.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Enums;
using Shared.Domain.Results;
using Shared.Infrastructure.Abstractions;

namespace Appointments.Application.Features.Commands.Appointments.CreateAppointment;

public sealed class CreateAppointmentCommandHandler : ICommandHandler<CreateAppointmentCommand, AppointmentCommandViewModel>
{
	private readonly IRepositoryManager _repositoryManager;
	private readonly IAppointmentService _appointmentService;
	private readonly IHAMSMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;
	public CreateAppointmentCommandHandler(IRepositoryManager repositoryManager, IAppointmentService appointmentServuce, IHAMSMapper mapper, IUnitOfWork unitOfWork)
	{
		_repositoryManager = repositoryManager;
		_appointmentService = appointmentServuce;
		_mapper = mapper;
		_unitOfWork = unitOfWork;
	}
	public async Task<Result<AppointmentCommandViewModel>> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
	{
		var doctorDataRes = await _repositoryManager.UserData.GetUserDataByEmailAsync(request.DoctorEmail);

		if (doctorDataRes.IsFailure)
			return Result<AppointmentCommandViewModel>.Failure(Responses.DoctorNotFound);
		if (doctorDataRes.Value!.Role != Roles.Doctor)
			return Result<AppointmentCommandViewModel>.Failure(Responses.UserIsNotADoctor);

		var patientDataRes = await _repositoryManager.UserData.GetUserDataByEmailAsync(request.PatientEmail);

		if (patientDataRes.IsFailure)
			return Result<AppointmentCommandViewModel>.Failure(Responses.PatientNotFound);

		var doctorPatientIdModel = new DoctorPatientIdModel(doctorDataRes.Value.UserId, patientDataRes.Value!.UserId);
		var createAppointmentModel = _mapper.Map<CreateAppointmentModel>((doctorPatientIdModel, request));
		var helperResult = await _appointmentService.CreateAppointment(createAppointmentModel);
		if (helperResult.IsFailure)
			return Result<AppointmentCommandViewModel>.Failure(helperResult.Response);

		await _unitOfWork.SaveChangesAsync();
		var appointmentCommandViewModel = helperResult.Value!;
		return Result<AppointmentCommandViewModel>.Success(appointmentCommandViewModel, Responses.AppointmentCreated);
	}
}
