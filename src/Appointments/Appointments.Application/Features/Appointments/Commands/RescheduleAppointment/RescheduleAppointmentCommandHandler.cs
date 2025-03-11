using Appointments.Application.Features.Appointments.Helpers.Abstractions;
using Appointments.Application.Features.Appointments.Models;
using Appointments.Application.Features.Jobs.Managers.Interfaces;
using Appointments.Domain.DTOS;
using Appointments.Domain.Enums;
using Appointments.Domain.Responses;
using Shared.Application.Abstractions;
using Shared.Application.Helpers.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Shared.Infrastructure.Abstractions;

namespace Appointments.Application.Features.Commands.Appointments.RescheduleAppointment;

public sealed class RescheduleAppointmentCommandHandler : ICommandHandler<RescheduleAppointmentCommand, AppointmentCommandViewModel>
{
	private readonly IRepositoryManager _repositoryManager;
	private readonly IJwtParser _jwtParser;
	private readonly IAppointmentService _appointmentService;
	private readonly IHAMSMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;
	public RescheduleAppointmentCommandHandler(IRepositoryManager repositoryManager, IJwtParser jwtParser, IAppointmentService appointmentServuce, IHAMSMapper mapper, IUnitOfWork unitOfWork)
	{
		_repositoryManager = repositoryManager;
		_jwtParser = jwtParser;
		_appointmentService = appointmentServuce;
		_mapper = mapper;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result<AppointmentCommandViewModel>> Handle(RescheduleAppointmentCommand request, CancellationToken cancellationToken)
	{
		var detailedAppointmentRes = await _repositoryManager.Appointment.GetAppointmentWithUserDetailsAsync(request.AppointmentID);

		if (detailedAppointmentRes.IsFailure)
			return Result<AppointmentCommandViewModel>.Failure(detailedAppointmentRes.Response);

		AppointmentWithDetailsModel appointmentWithDetails = detailedAppointmentRes.Value!;

		var userIdRes = _jwtParser.GetIdFromToken();

		if (userIdRes.IsFailure)
			return Result<AppointmentCommandViewModel>.Failure(userIdRes.Response);
		if (userIdRes.Value != appointmentWithDetails.PatientId && userIdRes.Value != appointmentWithDetails.DoctorId)
			return Result<AppointmentCommandViewModel>.Failure(Responses.CannotRescheduleOthersAppointment);

		var createAppointmentModel = _mapper.Map<CreateAppointmentModel>((appointmentWithDetails, request));
		var helperResult = await _appointmentService.CreateAppointment(createAppointmentModel);
		if (helperResult.IsFailure)
			return Result<AppointmentCommandViewModel>.Failure(helperResult.Response);

		_repositoryManager.Appointment.ChangeStatusAsync(appointmentWithDetails.Appointment, AppointmentStatus.Rescheduled);

		await _unitOfWork.SaveChangesAsync();
		var appointmentCommandViewModel = helperResult.Value!;
		return Result<AppointmentCommandViewModel>.Success(appointmentCommandViewModel, Responses.AppointmentCreated);
	}
}
