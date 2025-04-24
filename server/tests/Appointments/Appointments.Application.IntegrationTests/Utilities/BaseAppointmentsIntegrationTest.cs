using Appointments.Domain.Entities;
using Appointments.Domain.Entities.ValueObjects;
using Appointments.Domain.Infrastructure.Abstractions.Repository;
using Appointments.Domain.Utilities;
using Appointments.Infrastructure.Features.DBContexts;
using MassTransit.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.IntegrationTests.Utilities;
using Shared.Domain.Abstractions;

namespace Appointments.Application.IntegrationTests.Utilities;

public abstract class BaseAppointmentsIntegrationTest : BaseSharedIntegrationTest, IClassFixture<AppointmentsIntegrationTestWebAppFactory>, IAsyncLifetime
{
	protected BaseAppointmentsIntegrationTest(AppointmentsIntegrationTestWebAppFactory integrationTestWebAppFactory)
		: base(integrationTestWebAppFactory.Services.CreateScope())
	{
		AppointmentRepository = ServiceScope.ServiceProvider.GetRequiredService<IAppointmentRepository>();
	}

	protected IAppointmentRepository AppointmentRepository { get; }

	protected async Task<Appointment> CreateAppointmentAsync(bool isCompleted = false)
	{
		var unitOfWork = ServiceScope.ServiceProvider.GetRequiredService<IUnitOfWork>();

		var dateTimeRange = DateTimeRange.Create(
			AppointmentsTestUtilities.SoonDate,
			AppointmentsTestUtilities.FutureDate
		);

		var appointment = Appointment.Schedule(
			AppointmentsTestUtilities.PatientId,
			AppointmentsTestUtilities.DoctorId,
			dateTimeRange
		);

		if (isCompleted)
			appointment.Complete();

		await AppointmentRepository.AddAsync(appointment);
		await unitOfWork.SaveChangesAsync();

		return appointment;
	}

	public async Task DisposeAsync()
	{
		await EnsureDatabaseIsEmpty();
	}

	public async Task InitializeAsync()
	{
		await EnsureDatabaseIsEmpty();
	}

	private async Task EnsureDatabaseIsEmpty()
	{
		var dbContext = ServiceScope.ServiceProvider.GetRequiredService<AppointmentsDBContext>();

		if (dbContext.Appointments.Any())
		{
			await dbContext.Appointments.ExecuteDeleteAsync();
		}
	}
}
