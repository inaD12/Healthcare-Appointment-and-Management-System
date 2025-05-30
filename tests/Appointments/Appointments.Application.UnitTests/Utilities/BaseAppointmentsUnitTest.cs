using Appointments.Application.Features.Appointments.Helpers.Abstractions;
using Appointments.Application.Features.Appointments.Mappings;
using Appointments.Application.Features.Appointments.Models;
using Appointments.Application.Features.Jobs.Managers.Interfaces;
using Appointments.Application.Features.Mappings;
using Appointments.Domain.DTOS;
using Appointments.Domain.Entities;
using Appointments.Domain.Enums;
using Appointments.Domain.Responses;
using Appointments.Domain.Utilities;
using AutoMapper;
using NSubstitute;
using Shared.Application.Helpers;
using Shared.Application.Helpers.Abstractions;
using Shared.Application.UnitTests.Utilities;
using Shared.Domain.Enums;
using Shared.Domain.Results;

namespace Appointments.Application.UnitTests.Utilities;

public abstract class BaseAppointmentsUnitTest : BaseSharedUnitTest
{
	protected IRepositoryManager RepositoryMagager { get; }
	protected IJwtParser JWTParser { get; }
	protected IAppointmentService AppointmentService { get; }

	protected readonly List<Appointment> SceduledAppointmentList;

	protected BaseAppointmentsUnitTest() : base(
		new HAMSMapper(
			new Mapper(
				new MapperConfiguration(cfg =>
				{
					cfg.AddProfile<AppointmentCommandProfile>();
					cfg.AddProfile<AppointmentProfile>();
				}))))
	{
		
		RepositoryMagager = Substitute.For<IRepositoryManager>();
		JWTParser = Substitute.For<IJwtParser>();
		AppointmentService = Substitute.For<IAppointmentService>();

		var appointment = new Appointment(
			AppointmentsTestUtilities.ValidId,
			AppointmentsTestUtilities.ValidId,
			AppointmentsTestUtilities.SoonDate,
			AppointmentsTestUtilities.FutureDate,
			AppointmentStatus.Scheduled
			)
		{
			Id = AppointmentsTestUtilities.ValidId
		};

		SceduledAppointmentList = new List<Appointment> { appointment };

		var doctorData = new UserData
		(
			AppointmentsTestUtilities.DoctorId,
			AppointmentsTestUtilities.DoctorEmail,
			Roles.Doctor
		);

		var patientData = new UserData
		(
			AppointmentsTestUtilities.PatientId,
			AppointmentsTestUtilities.PatientEmail,
			Roles.Patient
		);

		var appointmentWithDetailsDTO = new AppointmentWithDetailsDTO
		{
			DoctorId = AppointmentsTestUtilities.ValidId,
			PatientId = AppointmentsTestUtilities.ValidId,
		};

		var appointmentWithDetailsDTONotMatching = new AppointmentWithDetailsDTO();

		string id = "";
		RepositoryMagager.Appointment.GetByIdAsync(Arg.Do<string>(a => id = a))
				.Returns(callInfo =>
				{
					if (id == AppointmentsTestUtilities.InvalidId)
						return (Result<Appointment>.Failure(Responses.AppointmentNotFound));

					return (Result<Appointment>.Success(appointment));
				});

		JWTParser.GetIdFromToken()
			.Returns(callInfo =>
			{
				if (id == AppointmentsTestUtilities.WrongIdFromTokenId)
					return Result<string>.Success(AppointmentsTestUtilities.InvalidId);
				if (id == AppointmentsTestUtilities.JWTExtractorInternalErrorId)
					return Result<string>.Failure(Responses.InternalError);

				return Result<string>.Success(AppointmentsTestUtilities.ValidId);
			});

		RepositoryMagager.Appointment.ChangeStatusAsync(Arg.Any<Appointment>(), (Arg.Any<AppointmentStatus>()))
			.Returns(callInfo =>
			{
				if (id == AppointmentsTestUtilities.ChangeStatusInternalErrorId)
					return (Result.Failure(Responses.InternalError));

				return Result.Success();
			});

		RepositoryMagager.Appointment.GetAppointmentsToCompleteAsync(Arg.Any<DateTime>())
		   .Returns(Result<List<Appointment>>.Success(SceduledAppointmentList));

		RepositoryMagager.Appointment.SaveChangesAsync().Returns(Task.CompletedTask);

		RepositoryMagager.UserData
			.GetUserDataByEmailAsync(Arg.Any<string>())
			.Returns(callInfo =>
			 {
				 string email = callInfo.ArgAt<string>(0);

				 if (email == AppointmentsTestUtilities.PatientEmail)
					 return Result<UserData>.Success(patientData);
				 if (email == AppointmentsTestUtilities.DoctorEmail)
					 return Result<UserData>.Success(doctorData);

				 return Result<UserData>.Failure(Responses.UserDataNotFound);
			 });

		AppointmentService.CreateAppointment(
			Arg.Any<CreateAppointmentModel>())
			.Returns(callInfo =>
			{
				string doctorid = callInfo.ArgAt<CreateAppointmentModel>(0).DoctorId;

				if (id == AppointmentsTestUtilities.HelperInternalErrorId)
					return Result.Failure(Responses.InternalError);

				return Result.Success();
			});

		RepositoryMagager.Appointment
			.GetAppointmentWithUserDetailsAsync(Arg.Do<string>(a => id = a))
			.Returns(callInfo =>
			 {
				 string id = callInfo.ArgAt<string>(0);

				 if (id == AppointmentsTestUtilities.InvalidId)
					 return Result<AppointmentWithDetailsDTO>.Failure(Responses.AppointmentNotFound);
				 if (id == AppointmentsTestUtilities.UnauthUserId)
					 return Result<AppointmentWithDetailsDTO>.Success(appointmentWithDetailsDTONotMatching);

				 return Result<AppointmentWithDetailsDTO>.Success(appointmentWithDetailsDTO);
			 });

		RepositoryMagager.Appointment
			.IsTimeSlotAvailableAsync(Arg.Any<string>(), Arg.Any<DateTime>(), Arg.Any<DateTime>())
			.Returns(callInfo =>
			{
				string id = callInfo.ArgAt<string>(0);

				if (id == AppointmentsTestUtilities.TimeSlotUnavailableId)
					return Result<bool>.Success(false);

				return Result<bool>.Success(true);
			});
	}


	protected void SetupGetAppointmentsToCompleteResult(Result<List<Appointment>> result)
	{
		RepositoryMagager.Appointment.GetAppointmentsToCompleteAsync(Arg.Any<DateTime>())
			.Returns(result);
	}
}
