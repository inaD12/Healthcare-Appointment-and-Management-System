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

public sealed class CreateAppointmentCommandHandler(
	IUnitOfWork unitOfWork,
	IAppointmentRepository appointmentRepository,
	IDoctorScheduleRepository doctorScheduleRepository,
	IRolesService rolesService)
	: ICommandHandler<CreateAppointmentCommand, AppointmentCommandViewModel>
{
	public async Task<Result<AppointmentCommandViewModel>> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
	{
		var doctorResult = await rolesService.GetUserRolesAsync(request.DoctorUserId, cancellationToken);
		
		if (doctorResult.IsFailure)
			return Result<AppointmentCommandViewModel>.Failure(ResponseList.DoctorNotFound);
		if (!doctorResult.Value!.Roles.Contains(nameof(Roles.Doctor)))
			return Result<AppointmentCommandViewModel>.Failure(ResponseList.UserIsNotADoctor);

		var patientData = await rolesService.GetUserRolesAsync(request.PatientUserId, cancellationToken);

		if (patientData.IsFailure)
			return Result<AppointmentCommandViewModel>.Failure(ResponseList.PatientNotFound);

		var duration = DateTimeRangeFactory.FromDuration(request.ScheduledStartTime, request.Duration);

		if (!await appointmentRepository.IsTimeSlotAvailableAsync(request.DoctorUserId, duration, cancellationToken))
			return Result<AppointmentCommandViewModel>.Failure(ResponseList.TimeSlotNotAvailable);
		
		var doctorSchedule = await doctorScheduleRepository.GetByIdAsync(request.DoctorUserId, cancellationToken);
		if (doctorSchedule == null)
			return Result<AppointmentCommandViewModel>.Failure(ResponseList.ScheduleNotFound);
		
		if(doctorSchedule.IsSlotAvailable(duration.Start, duration.End))
			return Result<AppointmentCommandViewModel>.Failure(ResponseList.NotWorkHours);
		
		try
		{
			var appointment = Appointment.Schedule(request.PatientUserId, request.DoctorUserId, duration);

			await appointmentRepository.AddAsync(appointment, cancellationToken);
			await unitOfWork.SaveChangesAsync(cancellationToken);

			var appointmentCommandViewModel = appointment.ToCommandViewModel();
			return Result<AppointmentCommandViewModel>.Success(appointmentCommandViewModel, ResponseList.AppointmentCreated);
		}
		catch (ConcurrencyException)
		{
			return Result<AppointmentCommandViewModel>.Failure(ResponseList.TimeSlotNotAvailable);
		}
	}
}
