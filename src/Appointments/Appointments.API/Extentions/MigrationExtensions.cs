using Appointments.Infrastructure.DBContexts;
using Microsoft.EntityFrameworkCore;

namespace Appointments.API.Extentions
{
	public static class MigrationExtensions
	{
		public static void ApplyMigrations(this IApplicationBuilder app)
		{
			using IServiceScope scope = app.ApplicationServices.CreateScope();

			using AppointmentsDBContext dBContext =
				scope.ServiceProvider.GetRequiredService<AppointmentsDBContext>();

			dBContext.Database.Migrate();
		}
	}
}
