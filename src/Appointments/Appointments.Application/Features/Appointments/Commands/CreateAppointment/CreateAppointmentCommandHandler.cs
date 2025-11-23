using Appointments.Application.Features.Appointments.Mappers;
using Appointments.Application.Features.Appointments.Models;
using Appointments.Domain.Abstractions;
using Appointments.Domain.Entities;
using Appointments.Domain.Utilities;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Enums;
using Shared.Domain.Exceptions;
using Shared.Domain.Results;

namespace Appointments.Application.Features.Appointments.Commands.CreateAppointment;

public sealed class CreateAppointmentCommandHandler : ICommandHandler<CreateAppointmentCommand, AppointmentCommandViewModel>
{
	private readonly IAppointmentRepository _appointmentRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IRolesService _rolesService;
	public CreateAppointmentCommandHandler(IUnitOfWork unitOfWork, IAppointmentRepository appointmentRepository, IRolesService rolesService)
	{
		_unitOfWork = unitOfWork;
		_appointmentRepository = appointmentRepository;
		_rolesService = rolesService;
	}
	public async Task<Result<AppointmentCommandViewModel>> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
	{
		var doctorResult = await _rolesService.GetUserRolesAsync(request.DoctorUserId, cancellationToken);
		
		if (doctorResult.IsFailure)
			return Result<AppointmentCommandViewModel>.Failure(ResponseList.DoctorNotFound);
		if (!doctorResult.Value!.Roles.Contains(nameof(Roles.Doctor)))
			return Result<AppointmentCommandViewModel>.Failure(ResponseList.UserIsNotADoctor);

		var patientData = await _rolesService.GetUserRolesAsync(request.PatientUserId, cancellationToken);

		if (patientData.IsFailure)
			return Result<AppointmentCommandViewModel>.Failure(ResponseList.PatientNotFound);

		var duration = DateTimeRangeFactory.FromDuration(request.ScheduledStartTime, request.Duration);

		if (!await _appointmentRepository.IsTimeSlotAvailableAsync(request.DoctorUserId, duration, cancellationToken))
			return Result<AppointmentCommandViewModel>.Failure(ResponseList.TimeSlotNotAvailable);

		try
		{
			var appointment = Appointment.Schedule(request.PatientUserId, request.DoctorUserId, duration);

			await _appointmentRepository.AddAsync(appointment, cancellationToken);
			await _unitOfWork.SaveChangesAsync(cancellationToken);

			var appointmentCommandViewModel = appointment.ToCommandViewModel();
			return Result<AppointmentCommandViewModel>.Success(appointmentCommandViewModel, ResponseList.AppointmentCreated);
		}
		catch (ConcurrencyException)
		{
			return Result<AppointmentCommandViewModel>.Failure(ResponseList.TimeSlotNotAvailable);
		}
	}
}
