using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Appointments.Infrastructure.Features.Migrations
{
    /// <inheritdoc />
    public partial class TimeSpan_To_TimeOnly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeeklySchedules_DoctorSchedules_DoctorScheduleDoctorId",
                table: "WeeklySchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorSchedules",
                table: "DoctorSchedules");

            migrationBuilder.RenameColumn(
                name: "DoctorScheduleDoctorId",
                table: "WeeklySchedules",
                newName: "DoctorScheduleId");

            migrationBuilder.RenameColumn(
                name: "DoctorId",
                table: "DoctorSchedules",
                newName: "Id");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "Start",
                table: "WorkTimeRanges",
                type: "time without time zone",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "interval");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "End",
                table: "WorkTimeRanges",
                type: "time without time zone",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "interval");

            migrationBuilder.AddPrimaryKey(
                name: "ID",
                table: "DoctorSchedules",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklySchedules_DoctorSchedules_DoctorScheduleId",
                table: "WeeklySchedules",
                column: "DoctorScheduleId",
                principalTable: "DoctorSchedules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeeklySchedules_DoctorSchedules_DoctorScheduleId",
                table: "WeeklySchedules");

            migrationBuilder.DropPrimaryKey(
                name: "ID",
                table: "DoctorSchedules");

            migrationBuilder.RenameColumn(
                name: "DoctorScheduleId",
                table: "WeeklySchedules",
                newName: "DoctorScheduleDoctorId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "DoctorSchedules",
                newName: "DoctorId");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Start",
                table: "WorkTimeRanges",
                type: "interval",
                nullable: false,
                oldClrType: typeof(TimeOnly),
                oldType: "time without time zone");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "End",
                table: "WorkTimeRanges",
                type: "interval",
                nullable: false,
                oldClrType: typeof(TimeOnly),
                oldType: "time without time zone");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorSchedules",
                table: "DoctorSchedules",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklySchedules_DoctorSchedules_DoctorScheduleDoctorId",
                table: "WeeklySchedules",
                column: "DoctorScheduleDoctorId",
                principalTable: "DoctorSchedules",
                principalColumn: "DoctorId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
