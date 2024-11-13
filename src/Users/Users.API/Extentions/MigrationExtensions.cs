using Microsoft.EntityFrameworkCore;
using Users.Infrastructure.UsersDBContexts;

namespace Healthcare_Appointment_and_Management_System.Extentions
{
	public static class MigrationExtensions
	{
		public static void ApplyMigrations(this IApplicationBuilder app)
		{
			using IServiceScope scope = app.ApplicationServices.CreateScope();

			using UsersDBContext dBContext =
				scope.ServiceProvider.GetRequiredService<UsersDBContext>();

			dBContext.Database.Migrate();
		}
	}
}
