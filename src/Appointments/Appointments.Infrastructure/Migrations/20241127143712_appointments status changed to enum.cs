using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Appointments.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class appointmentsstatuschangedtoenum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql("CREATE TYPE appointment_status AS ENUM ('Scheduled', 'Rescheduled', 'Cancelled', 'Completed');");

			migrationBuilder.DropColumn(
				name: "Status",
				table: "Appointments");

			migrationBuilder.AddColumn<string>(
				name: "Status",
				table: "Appointments",
				type: "appointment_status",
				nullable: false,
				defaultValue: "Scheduled");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.DropColumn(
			name: "Status",
			table: "Appointments");

			migrationBuilder.Sql("DROP TYPE appointment_status;");

			migrationBuilder.AddColumn<int>(
		   name: "Status",
		   table: "Appointments",
		   type: "integer",
		   nullable: false,
		   defaultValue: 0);
		}
    }
}
