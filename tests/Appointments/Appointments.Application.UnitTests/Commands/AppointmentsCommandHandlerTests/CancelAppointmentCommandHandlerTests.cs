using Appointments.Application.Appoints.Commands.CancelAppointment;
using Appointments.Application.Helpers;
using Appointments.Application.Managers.Interfaces;
using Appointments.Domain.Entities;
using Contracts.Results;
using NSubstitute;
using FluentAssertions;
using Appointments.Domain.Responses;

namespace Appointments.Application.UnitTests.Commands.AppointmentsCommandHandlerTests
{
	public class CancelAppointmentCommandHandlerTests
	{
		private readonly IRepositoryManager _mockRepositoryManager;
		private readonly IJWTUserExtractor _mockJwtUserExtractor;
		private readonly CancelAppointmentCommandHandler _handler;

		private readonly string AppointmentId;
		private readonly string PatientId;
		private readonly Appointment Appointment;


		public CancelAppointmentCommandHandlerTests()
		{
			_mockRepositoryManager = Substitute.For<IRepositoryManager>();
			_mockJwtUserExtractor = Substitute.For<IJWTUserExtractor>();
			_handler = new CancelAppointmentCommandHandler(_mockRepositoryManager, _mockJwtUserExtractor);

			AppointmentId = Guid.NewGuid().ToString();
			PatientId = Guid.NewGuid().ToString();

			Appointment = new Appointment(
				AppointmentId,
				PatientId,
				Guid.NewGuid().ToString(),
				DateTime.UtcNow,
				DateTime.UtcNow.AddMinutes(15),
				Domain.Enums.AppointmentStatus.Scheduled);
		}

		[Fact]
		public async Task Handle_ShouldReturnSuccess_WhenCalcelationIsSuccessful()
		{
			//Arrange
			_mockRepositoryManager.Appointment.GetByIdAsync(AppointmentId)
				.Returns(Result<Appointment>.Success(Appointment));

			_mockJwtUserExtractor.GetUserIdFromTokenAsync()
				.Returns(Result<string>.Success(PatientId));

			_mockRepositoryManager.Appointment.ChangeStatusAsync(Appointment, Domain.Enums.AppointmentStatus.Cancelled)
				.Returns(Result.Success());

			var command = new CancelAppointmentCommand(AppointmentId);

			//Act
			var result = await _handler.Handle(command, CancellationToken.None);

			//Assert
			result.IsSuccess.Should().BeTrue();

			await _mockRepositoryManager.Appointment.Received(1).GetByIdAsync(AppointmentId);
			await _mockJwtUserExtractor.Received(1).GetUserIdFromTokenAsync();
			await _mockRepositoryManager.Appointment.Received(1).ChangeStatusAsync(Appointment, Domain.Enums.AppointmentStatus.Cancelled);
		}

		[Fact]
		public async Task Handle_ShouldReturnFaliure_WhenAppointmentDoesntExist()
		{
			//Arrange
			_mockRepositoryManager.Appointment.GetByIdAsync(AppointmentId)
				.Returns(Result<Appointment>.Success(Appointment));

			_mockJwtUserExtractor.GetUserIdFromTokenAsync()
				.Returns(Result<string>.Success("WrongId"));

			_mockRepositoryManager.Appointment.ChangeStatusAsync(Appointment, Domain.Enums.AppointmentStatus.Cancelled)
				.Returns(Result.Success());

			var command = new CancelAppointmentCommand(AppointmentId);

			//Act
			var result = await _handler.Handle(command, CancellationToken.None);

			//Assert
			result.IsFailure.Should().BeTrue();
			result.Response.Should().BeEquivalentTo(Responses.CannotCancelOthersAppointment);

			await _mockRepositoryManager.Appointment.Received(1).GetByIdAsync(AppointmentId);
			await _mockJwtUserExtractor.Received(1).GetUserIdFromTokenAsync();
			await _mockRepositoryManager.Appointment.Received(0).ChangeStatusAsync(Appointment, Domain.Enums.AppointmentStatus.Cancelled);
		}

		[Fact]
		public async Task Handle_ShouldReturnFaliure_WhenIdsDontMatch()
		{
			//Arrange
			_mockRepositoryManager.Appointment.GetByIdAsync(AppointmentId)
				.Returns(Result<Appointment>.Failure(Responses.AppointmentNotFound));

			_mockJwtUserExtractor.GetUserIdFromTokenAsync()
				.Returns(Result<string>.Success(PatientId));

			_mockRepositoryManager.Appointment.ChangeStatusAsync(Appointment, Domain.Enums.AppointmentStatus.Cancelled)
				.Returns(Result.Success());

			var command = new CancelAppointmentCommand(AppointmentId);

			//Act
			var result = await _handler.Handle(command, CancellationToken.None);

			//Assert
			result.IsFailure.Should().BeTrue();
			result.Response.Should().BeEquivalentTo(Responses.AppointmentNotFound);

			await _mockRepositoryManager.Appointment.Received(1).GetByIdAsync(AppointmentId);
			await _mockJwtUserExtractor.Received(0).GetUserIdFromTokenAsync();
			await _mockRepositoryManager.Appointment.Received(0).ChangeStatusAsync(Appointment, Domain.Enums.AppointmentStatus.Cancelled);
		}

		[Fact]
		public async Task Handle_ShouldReturnFailure_WhenCalcelationIsSuccessful()
		{
			//Arrange
			_mockRepositoryManager.Appointment.GetByIdAsync(AppointmentId)
				.Returns(Result<Appointment>.Success(Appointment));

			_mockJwtUserExtractor.GetUserIdFromTokenAsync()
				.Returns(Result<string>.Failure(Responses.InternalError));

			_mockRepositoryManager.Appointment.ChangeStatusAsync(Appointment, Domain.Enums.AppointmentStatus.Cancelled)
				.Returns(Result.Success());

			var command = new CancelAppointmentCommand(AppointmentId);

			//Act
			var result = await _handler.Handle(command, CancellationToken.None);

			//Assert
			result.IsFailure.Should().BeTrue();
			result.Response.Should().BeEquivalentTo(Responses.InternalError);

			await _mockRepositoryManager.Appointment.Received(1).GetByIdAsync(AppointmentId);
			await _mockJwtUserExtractor.Received(1).GetUserIdFromTokenAsync();
			await _mockRepositoryManager.Appointment.Received(0).ChangeStatusAsync(Appointment, Domain.Enums.AppointmentStatus.Cancelled);
		}

		[Fact]
		public async Task Handle_ShouldReturnFailure_WhenStatusChangingFails()
		{
			//Arrange
			_mockRepositoryManager.Appointment.GetByIdAsync(AppointmentId)
				.Returns(Result<Appointment>.Success(Appointment));

			_mockJwtUserExtractor.GetUserIdFromTokenAsync()
				.Returns(Result<string>.Success(PatientId));

			_mockRepositoryManager.Appointment.ChangeStatusAsync(Appointment, Domain.Enums.AppointmentStatus.Cancelled)
				.Returns(Result.Failure(Responses.InternalError));

			var command = new CancelAppointmentCommand(AppointmentId);

			//Act
			var result = await _handler.Handle(command, CancellationToken.None);

			//Assert
			result.IsFailure.Should().BeTrue();
			result.Response.Should().BeEquivalentTo(Responses.InternalError);

			await _mockRepositoryManager.Appointment.Received(1).GetByIdAsync(AppointmentId);
			await _mockJwtUserExtractor.Received(1).GetUserIdFromTokenAsync();
			await _mockRepositoryManager.Appointment.Received(1).ChangeStatusAsync(Appointment, Domain.Enums.AppointmentStatus.Cancelled);
		}
	}
}
