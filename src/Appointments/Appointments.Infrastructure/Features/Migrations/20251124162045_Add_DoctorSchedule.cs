using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Appointments.Infrastructure.Features.Migrations
{
    /// <inheritdoc />
    public partial class Add_DoctorSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DoctorSchedules",
                columns: table => new
                {
                    DoctorId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorSchedules", x => x.DoctorId);
                });

            migrationBuilder.CreateTable(
                name: "DoctorAvailabilityExceptions",
                columns: table => new
                {
                    DoctorId = table.Column<string>(type: "text", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Start = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    End = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Reason = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorAvailabilityExceptions", x => new { x.DoctorId, x.Id });
                    table.ForeignKey(
                        name: "FK_DoctorAvailabilityExceptions_DoctorSchedules_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "DoctorSchedules",
                        principalColumn: "DoctorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeeklySchedules",
                columns: table => new
                {
                    DoctorScheduleDoctorId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklySchedules", x => x.DoctorScheduleDoctorId);
                    table.ForeignKey(
                        name: "FK_WeeklySchedules_DoctorSchedules_DoctorScheduleDoctorId",
                        column: x => x.DoctorScheduleDoctorId,
                        principalTable: "DoctorSchedules",
                        principalColumn: "DoctorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkDays",
                columns: table => new
                {
                    WeeklyScheduleId = table.Column<string>(type: "text", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DayOfWeek = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkDays", x => new { x.WeeklyScheduleId, x.Id });
                    table.ForeignKey(
                        name: "FK_WorkDays_WeeklySchedules_WeeklyScheduleId",
                        column: x => x.WeeklyScheduleId,
                        principalTable: "WeeklySchedules",
                        principalColumn: "DoctorScheduleDoctorId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkTimeRanges",
                columns: table => new
                {
                    WorkDayScheduleWeeklyScheduleId = table.Column<string>(type: "text", nullable: false),
                    WorkDayScheduleId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Start = table.Column<TimeSpan>(type: "interval", nullable: false),
                    End = table.Column<TimeSpan>(type: "interval", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTimeRanges", x => new { x.WorkDayScheduleWeeklyScheduleId, x.WorkDayScheduleId, x.Id });
                    table.ForeignKey(
                        name: "FK_WorkTimeRanges_WorkDays_WorkDayScheduleWeeklyScheduleId_Wor~",
                        columns: x => new { x.WorkDayScheduleWeeklyScheduleId, x.WorkDayScheduleId },
                        principalTable: "WorkDays",
                        principalColumns: new[] { "WeeklyScheduleId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DoctorAvailabilityExceptions");

            migrationBuilder.DropTable(
                name: "WorkTimeRanges");

            migrationBuilder.DropTable(
                name: "WorkDays");

            migrationBuilder.DropTable(
                name: "WeeklySchedules");

            migrationBuilder.DropTable(
                name: "DoctorSchedules");
        }
    }
}
