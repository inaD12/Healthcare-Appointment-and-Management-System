using Appointments.Domain.Abstractions;
using Appointments.Domain.Entities;
using Appointments.Infrastructure.Features.DBContexts;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Repositories;

namespace Appointments.Infrastructure.Features.Repositories;

internal class DoctorScheduleRepository(AppointmentsDBContext context)
	: GenericRepository<DoctorSchedule>(context), IDoctorScheduleRepository
{
	public override async Task<DoctorSchedule?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
	{
		return await context.DoctorSchedule
			.Include(s => s.WeeklySchedule)
				.ThenInclude(w => w.WorkDays)
				.ThenInclude(d => d.WorkTimes)
			.Include(s => s.AvailabilityExceptions)
			.FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
	}
}
