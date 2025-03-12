using Appointments.Domain.Entities.Enums;
using Appointments.Domain.Entities.ValueObjects;
using Appointments.Domain.Responses;
using Shared.Domain.Entities.Base;
using Shared.Domain.Results;

namespace Appointments.Domain.Entities;

public sealed class Appointment : BaseEntity
{
	private Appointment(string patientId, string doctorId, AppointmentStatus status, DateTimeRange duration)
	{
		PatientId = patientId;
		DoctorId = doctorId;
		Status = status;
		Duration = duration;
	}

	private Appointment()
	{
	}

	public string PatientId { get; private set; }
	public string DoctorId { get; private set; }
	public DateTimeRange Duration { get; private set; }
	public AppointmentStatus Status { get; private set; }

	public static Appointment Schedule(string patientId, string doctorId, DateTimeRange duration)
	{
		var appointment =  new Appointment(
			patientId, doctorId,
			AppointmentStatus.Scheduled,
			duration);

		return appointment;
	}

	public Result Reschedule(DateTime utcNow)
	{
		if (Status != AppointmentStatus.Scheduled)
			return Result.Failure(ResponseList.AppointmentNotScheduled);

		if (utcNow > Duration.Start)
			return Result.Failure(ResponseList.AppointmentAlreadyStarted);

		Status = AppointmentStatus.Rescheduled;

		return Result.Success();
	}

	public Result Cancel(DateTime utcNow)
	{
		if (Status != AppointmentStatus.Scheduled)
			return Result.Failure(ResponseList.AppointmentNotScheduled);

		if (utcNow > Duration.Start)
			return Result.Failure(ResponseList.AppointmentAlreadyStarted);

		Status = AppointmentStatus.Cancelled;

		return Result.Success();
	}

	public Result Complete()
	{
		if (Status != AppointmentStatus.Scheduled)
			return Result.Failure(ResponseList.AppointmentNotScheduled);

		Status = AppointmentStatus.Completed;

		return Result.Success();
	}
}
