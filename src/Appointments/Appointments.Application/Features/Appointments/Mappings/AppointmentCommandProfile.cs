using Appointments.Application.Features.Appointments.Models;
using Appointments.Application.Features.Commands.Appointments.CreateAppointment;
using Appointments.Application.Features.Commands.Appointments.RescheduleAppointment;
using Appointments.Domain.DTOS;
using Appointments.Domain.Entities;
using Appointments.Domain.Enums;
using AutoMapper;

namespace Appointments.Application.Features.Appointments.Mappings;

public class AppointmentCommandProfile : Profile
{
	public AppointmentCommandProfile()
	{
		CreateMap<CreateAppointmentModel, Appointment>()
				.ConstructUsing(src => new(
					src.PatientId,
					src.DoctorId,
					src.StartTime,
					src.StartTime.AddMinutes((int)src.Duration),
					src.Status
					));

		CreateMap<(AppointmentWithDetailsDTO, RescheduleAppointmentCommand), CreateAppointmentModel>()
				.ConstructUsing(src => new(
					src.Item1.DoctorEmail,
					src.Item1.PatientEmail,
					src.Item2.ScheduledStartTime.ToUniversalTime(),
					src.Item2.Duration,
					AppointmentStatus.Scheduled
					));

		CreateMap<(DoctorPatientIdModel, CreateAppointmentCommand), CreateAppointmentModel>()
				.ConstructUsing(src => new(
					src.Item1.DoctorId,
					src.Item1.PatientId,
					src.Item2.ScheduledStartTime.ToUniversalTime(),
					src.Item2.Duration,
					AppointmentStatus.Scheduled
					));

		CreateMap<Appointment, AppointmentCommandViewModel>();
	}
}