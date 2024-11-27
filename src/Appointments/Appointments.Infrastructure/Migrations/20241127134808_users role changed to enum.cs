using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Appointments.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class usersrolechangedtoenum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql("CREATE TYPE roles AS ENUM ('Patient', 'Doctor', 'Admin');");

			migrationBuilder.DropColumn(
				name: "Role",
				table: "UserData");

			migrationBuilder.AddColumn<string>(
				name: "Role",
				table: "UserData",
				type: "roles",
				nullable: false,
				defaultValue: "Patient");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "Role",
				table: "UserData");

			migrationBuilder.Sql("DROP TYPE roles;");

			migrationBuilder.AddColumn<string>(
				name: "Role",
				table: "UserData",
				type: "text",
				nullable: false);
		}
	}
}
