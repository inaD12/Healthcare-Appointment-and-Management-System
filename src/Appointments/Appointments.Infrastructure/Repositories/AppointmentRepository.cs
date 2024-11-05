using Appointments.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Appointments.Infrastructure.Repositories
{
	internal class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
	{
		public AppointmentRepository(DbContext context) : base(context)
		{
		}
	}
}
