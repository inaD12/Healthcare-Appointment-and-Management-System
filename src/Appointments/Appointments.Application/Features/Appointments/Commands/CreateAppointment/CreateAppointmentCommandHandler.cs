using Appointments.Application.Features.Appointments.Models;
using Appointments.Domain.Entities;
using Appointments.Domain.Entities.ValueObjects;
using Appointments.Domain.Infrastructure.Abstractions.Repository;
using Appointments.Domain.Responses;
using Shared.Application.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Enums;
using Shared.Domain.Results;
using Shared.Infrastructure.Abstractions;

namespace Appointments.Application.Features.Commands.Appointments.CreateAppointment;

public sealed class CreateAppointmentCommandHandler : ICommandHandler<CreateAppointmentCommand, AppointmentCommandViewModel>
{
	private readonly IUserDataRepository _userDataRepository;
	private readonly IAppointmentRepository _appointmentRepository;
	private readonly IHAMSMapper _mapper;
	private readonly IUnitOfWork _unitOfWork;
	public CreateAppointmentCommandHandler(IHAMSMapper mapper, IUnitOfWork unitOfWork, IUserDataRepository userDataRepository, IAppointmentRepository appointmentRepository)
	{
		_mapper = mapper;
		_unitOfWork = unitOfWork;
		_userDataRepository = userDataRepository;
		_appointmentRepository = appointmentRepository;
	}
	public async Task<Result<AppointmentCommandViewModel>> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
	{
		var doctorDataRes = await _userDataRepository.GetUserDataByEmailAsync(request.DoctorEmail);

		if (doctorDataRes.IsFailure)
			return Result<AppointmentCommandViewModel>.Failure(ResponseList.DoctorNotFound);
		if (doctorDataRes.Value!.Role != Roles.Doctor)
			return Result<AppointmentCommandViewModel>.Failure(ResponseList.UserIsNotADoctor);

		var patientDataRes = await _userDataRepository.GetUserDataByEmailAsync(request.PatientEmail);

		if (patientDataRes.IsFailure)
			return Result<AppointmentCommandViewModel>.Failure(ResponseList.PatientNotFound);

		var duration = DateTimeRange.Create(request.ScheduledStartTime, request.Duration);

		if(await _appointmentRepository.IsTimeSlotAvailableAsync(doctorDataRes.Value.Id, duration, cancellationToken))
			return Result<AppointmentCommandViewModel>.Failure(ResponseList.TimeSlotNotAvailable);

		var appointment = Appointment.Schedule(patientDataRes.Value!.UserId, doctorDataRes.Value.UserId, duration);

		await _appointmentRepository.AddAsync(appointment);
		await _unitOfWork.SaveChangesAsync();

		var appointmentCommandViewModel = _mapper.Map<AppointmentCommandViewModel>(appointment);
		return Result<AppointmentCommandViewModel>.Success(appointmentCommandViewModel, ResponseList.AppointmentCreated);
	}
}
