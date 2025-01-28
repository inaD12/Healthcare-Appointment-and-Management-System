using Appointments.Application.Appoints.Commands.CreateAppointment;
using Appointments.Application.Appoints.Commands.Shared;
using Appointments.Application.Managers.Interfaces;
using Appointments.Domain.Entities;
using Appointments.Domain.Responses;
using Contracts.Results;
using FluentAssertions;
using NSubstitute;

namespace Appointments.Application.UnitTests.Commands.AppointmentsCommandHandlerTests
{
	public class CreateAppointmentCommandHandlerTests
	{
		private readonly IRepositoryManager _mockRepositoryManager;
		private readonly IAppointmentCommandHandlerHelper _mockHelper;
		private readonly CreateAppointmentCommandHandler _handler;

		private readonly string DoctorEmai;
		private readonly string PatientEmal;
		private readonly CreateAppointmentCommand Command;
		private readonly UserData PatientData;
		private readonly UserData DoctorData;
		public CreateAppointmentCommandHandlerTests()
		{
			_mockRepositoryManager = Substitute.For<IRepositoryManager>();
			_mockHelper = Substitute.For<IAppointmentCommandHandlerHelper>();

			_handler = new CreateAppointmentCommandHandler(_mockRepositoryManager, _mockHelper);

			DoctorEmai = "doctor@example.com";
			PatientEmal = "patient@example.com";
			DoctorData = new UserData { UserId = Guid.NewGuid().ToString(), Email = "doctor@example.com" };
			PatientData = new UserData { UserId = Guid.NewGuid().ToString(), Email = "patient@example.com" };

			Command = new CreateAppointmentCommand
			(
				PatientEmal,
				DoctorEmai,
				DateTime.UtcNow,
				Domain.Enums.AppointmentDuration.OneHour
			);
		}

		[Fact]
		public async Task Handle_ShouldReturnFailure_WhenDoctorNotFound()
		{
			// Arrange

			_mockRepositoryManager.UserData
				.GetUserDataByEmailAsync(Command.DoctorEmail)
				.Returns(Result<UserData>.Failure(Responses.UserDataNotFound));

			var cancellationToken = CancellationToken.None;

			// Act
			var result = await _handler.Handle(Command, cancellationToken);

			// Assert
			result.IsSuccess.Should().BeFalse();
			result.Response.Should().BeEquivalentTo(Responses.DoctorNotFound);

			await _mockRepositoryManager.UserData.DidNotReceive().GetUserDataByEmailAsync(Command.PatientEmail);
		}

		[Fact]
		public async Task Handle_ShouldReturnFailure_WhenPatientNotFound()
		{
			// Arrange
			_mockRepositoryManager.UserData
				.GetUserDataByEmailAsync(Command.DoctorEmail)
				.Returns(Result<UserData>.Success(DoctorData));

			_mockRepositoryManager.UserData
				.GetUserDataByEmailAsync(Command.PatientEmail)
				.Returns(Result<UserData>.Failure(Responses.UserDataNotFound));

			var cancellationToken = CancellationToken.None;

			// Act
			var result = await _handler.Handle(Command, cancellationToken);

			// Assert
			result.IsSuccess.Should().BeFalse();
			result.Response.Should().BeEquivalentTo(Responses.PatientNotFound);

			await _mockHelper.DidNotReceiveWithAnyArgs().CreateAppointment(default, default, default, default);
		}

		[Fact]
		public async Task Handle_ShouldCallHelperAndReturnResult_WhenDoctorAndPatientExist()
		{
			// Arrange
			_mockRepositoryManager.UserData
				.GetUserDataByEmailAsync(Command.DoctorEmail)
				.Returns(Result<UserData>.Success(DoctorData));

			_mockRepositoryManager.UserData
				.GetUserDataByEmailAsync(Command.PatientEmail)
				.Returns(Result<UserData>.Success(PatientData));

			_mockHelper.CreateAppointment(
				DoctorData.UserId,
				PatientData.UserId,
				Command.ScheduledStartTime.ToUniversalTime(),
				Command.Duration)
				.Returns(Result.Success());

			var cancellationToken = CancellationToken.None;

			// Act
			var result = await _handler.Handle(Command, cancellationToken);

			// Assert
			result.IsSuccess.Should().BeTrue();

			await _mockHelper.Received(1).CreateAppointment(
				DoctorData.UserId,
				PatientData.UserId,
				Command.ScheduledStartTime.ToUniversalTime(),
				Command.Duration);
		}
	}
}
