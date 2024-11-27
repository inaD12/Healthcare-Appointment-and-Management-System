using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Users.Infrastructure.Migrations
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
				table: "Users");

			migrationBuilder.AddColumn<string>(
				name: "Role",
				table: "Users",
				type: "roles",
				nullable: false,
				defaultValue: "Patient");

			migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(450)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Users",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "EmailVerificationTokens",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(450)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiresOnUtc",
                table: "EmailVerificationTokens",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOnUtc",
                table: "EmailVerificationTokens",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "EmailVerificationTokens",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(450)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.DropColumn(
				name: "Role",
				table: "Users");

			migrationBuilder.Sql("DROP TYPE roles;");

			migrationBuilder.AddColumn<string>(
				name: "Role",
				table: "Users",
				type: "text",
				nullable: false,
				defaultValue: "Patient");

			migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "varchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfBirth",
                table: "Users",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Users",
                type: "varchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "EmailVerificationTokens",
                type: "varchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ExpiresOnUtc",
                table: "EmailVerificationTokens",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedOnUtc",
                table: "EmailVerificationTokens",
                type: "timestamp",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "EmailVerificationTokens",
                type: "varchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
