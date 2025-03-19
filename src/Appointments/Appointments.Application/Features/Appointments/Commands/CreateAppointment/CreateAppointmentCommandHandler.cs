using Appointments.Application.Features.Appointments.Models;
using Appointments.Application.Features.Commands.Appointments.CreateAppointment;
using Appointments.Domain.Entities;
using Appointments.Domain.Entities.ValueObjects;
using Appointments.Domain.Infrastructure.Abstractions.Repository;
using Appointments.Domain.Responses;
using Shared.Application.Abstractions;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Enums;
using Shared.Domain.Exceptions;
using Shared.Domain.Results;

namespace Appointments.Application.Features.Appointments.Commands.CreateAppointment;

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
		var doctorData = await _userDataRepository.GetUserDataByEmailAsync(request.DoctorEmail);

		if (doctorData == null)
			return Result<AppointmentCommandViewModel>.Failure(ResponseList.DoctorNotFound);
		if (doctorData.Role != Roles.Doctor)
			return Result<AppointmentCommandViewModel>.Failure(ResponseList.UserIsNotADoctor);

		var patientData = await _userDataRepository.GetUserDataByEmailAsync(request.PatientEmail);

		if (patientData == null)
			return Result<AppointmentCommandViewModel>.Failure(ResponseList.PatientNotFound);

		var duration = DateTimeRange.Create(request.ScheduledStartTime, request.Duration);

		if (await _appointmentRepository.IsTimeSlotAvailableAsync(doctorData.Id, duration, cancellationToken))
			return Result<AppointmentCommandViewModel>.Failure(ResponseList.TimeSlotNotAvailable);

		try
		{
			var appointment = Appointment.Schedule(patientData.UserId, doctorData.UserId, duration);

			await _appointmentRepository.AddAsync(appointment);
			await _unitOfWork.SaveChangesAsync();

			var appointmentCommandViewModel = _mapper.Map<AppointmentCommandViewModel>(appointment);
			return Result<AppointmentCommandViewModel>.Success(appointmentCommandViewModel, ResponseList.AppointmentCreated);
		}
		catch (ConcurrencyException)
		{
			return Result<AppointmentCommandViewModel>.Failure(ResponseList.TimeSlotNotAvailable);
		}
	}
}
