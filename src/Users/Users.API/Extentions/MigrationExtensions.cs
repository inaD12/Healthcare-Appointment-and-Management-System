using MassTransit;
using Microsoft.EntityFrameworkCore;
using Users.Infrastructure.UsersDBContexts;

namespace UsersAPI.Extentions
{
	public static class MigrationExtensions
	{
		public async static Task ApplyMigrations(this IApplicationBuilder app)
		{

			using IServiceScope scope = app.ApplicationServices.CreateScope();

			using UsersDBContext dBContext =
				scope.ServiceProvider.GetRequiredService<UsersDBContext>();

			var pendingMigrations = await dBContext.Database.GetPendingMigrationsAsync();

			if (pendingMigrations.Any())
				await dBContext.Database.MigrateAsync();
		}
	}
}
