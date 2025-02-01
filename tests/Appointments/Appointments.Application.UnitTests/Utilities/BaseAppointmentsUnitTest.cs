using Appointments.Application.Appoints.Commands.Shared;
using Appointments.Application.Helpers;
using Appointments.Application.Managers.Interfaces;
using Appointments.Domain.DTOS;
using Appointments.Domain.Entities;
using Appointments.Domain.Enums;
using Appointments.Domain.Responses;
using Appointments.Domain.Utilities;
using Contracts.Results;
using NSubstitute;

namespace Appointments.Application.UnitTests.Utilities;

public abstract class BaseAppointmentsUnitTest
{
	protected IRepositoryManager RepositoryMagager { get; }
	protected IFactoryManager FactoryMagager { get; }
	protected IJWTUserExtractor JWTUserExtractor { get; }
	protected IAppointmentCommandHandlerHelper AppointmentCommandHandlerHelper { get; }

	protected readonly List<Appointment> SceduledAppointmentList;

	protected BaseAppointmentsUnitTest()
	{
		RepositoryMagager = Substitute.For<IRepositoryManager>();
		FactoryMagager = Substitute.For<IFactoryManager>();
		JWTUserExtractor = Substitute.For<IJWTUserExtractor>();
		AppointmentCommandHandlerHelper = Substitute.For<IAppointmentCommandHandlerHelper>();

		var appointment = new Appointment(
			AppointmentsTestUtilities.ValidId,
			AppointmentsTestUtilities.ValidId,
			AppointmentsTestUtilities.ValidId,
			AppointmentsTestUtilities.SoonDate,
			AppointmentsTestUtilities.FutureDate,
			AppointmentStatus.Scheduled
			);

		SceduledAppointmentList = new List<Appointment> { appointment };

		var doctorData = new UserData
		{
			UserId = AppointmentsTestUtilities.DoctorId,
			Email = AppointmentsTestUtilities.DoctorEmail,
			Role = Contracts.Enums.Roles.Doctor
		};

		var patientData = new UserData
		{
			UserId = AppointmentsTestUtilities.PatientId,
			Email = AppointmentsTestUtilities.PatientEmail,
			Role = Contracts.Enums.Roles.Patient
		};

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

		JWTUserExtractor.GetUserIdFromTokenAsync()
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

		AppointmentCommandHandlerHelper.CreateAppointment(
			Arg.Any<string>(),
			Arg.Any<string>(),
			Arg.Any<DateTime>(),
			Arg.Any<AppointmentDuration>())
			.Returns(callInfo =>
			{
				string doctorid = callInfo.ArgAt<string>(0);

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
				 if(id == AppointmentsTestUtilities.UnauthUserId)
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

		FactoryMagager.Appointment.Create(
				Arg.Any<string>(),
				Arg.Any<string>(),
				Arg.Any<DateTime>(),
				Arg.Any<DateTime>())
			.Returns(appointment);
	}


	protected void SetupGetAppointmentsToCompleteResult(Result<List<Appointment>> result)
	{
		RepositoryMagager.Appointment.GetAppointmentsToCompleteAsync(Arg.Any<DateTime>())
			.Returns(result);
	}
}
