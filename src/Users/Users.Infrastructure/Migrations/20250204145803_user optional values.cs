using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Users.Infrastructure.Migrations;

/// <inheritdoc />
public partial class useroptionalvalues : Migration
{
	/// <inheritdoc />
	protected override void Up(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AlterColumn<string>(
			name: "PhoneNumber",
			table: "Users",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "text");

		migrationBuilder.AlterColumn<string>(
			name: "Address",
			table: "Users",
			type: "text",
			nullable: true,
			oldClrType: typeof(string),
			oldType: "text");
	}

	/// <inheritdoc />
	protected override void Down(MigrationBuilder migrationBuilder)
	{
		migrationBuilder.AlterColumn<string>(
			name: "PhoneNumber",
			table: "Users",
			type: "text",
			nullable: false,
			defaultValue: "",
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);

		migrationBuilder.AlterColumn<string>(
			name: "Address",
			table: "Users",
			type: "text",
			nullable: false,
			defaultValue: "",
			oldClrType: typeof(string),
			oldType: "text",
			oldNullable: true);
	}
}
