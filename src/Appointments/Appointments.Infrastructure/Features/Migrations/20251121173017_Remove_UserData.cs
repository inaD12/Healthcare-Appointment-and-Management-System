using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Shared.Domain.Enums;

#nullable disable

namespace Appointments.Infrastructure.Features.Migrations
{
    /// <inheritdoc />
    public partial class Remove_UserData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserData");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserData",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Roles = table.Column<List<Roles>>(type: "roles[]", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserData", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserData_Id",
                table: "UserData",
                column: "Id",
                unique: true);
        }
    }
}
