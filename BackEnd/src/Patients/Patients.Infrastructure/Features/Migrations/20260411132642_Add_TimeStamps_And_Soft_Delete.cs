using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Patients.Infrastructure.Features.Migrations
{
    /// <inheritdoc />
    public partial class Add_TimeStamps_And_Soft_Delete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Encounters",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "EncounterPrescriptions",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "EncounterPrescriptions",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EncounterPrescriptions",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "EncounterNotes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EncounterNotes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "EncounterDiagnoses",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "EncounterDiagnoses",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EncounterDiagnoses",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "EncounterAddendums",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "EncounterAddendums",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Encounters");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "EncounterPrescriptions");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "EncounterPrescriptions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "EncounterPrescriptions");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "EncounterNotes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "EncounterNotes");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "EncounterDiagnoses");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "EncounterDiagnoses");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "EncounterDiagnoses");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "EncounterAddendums");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "EncounterAddendums");
        }
    }
}
