using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Users.Infrastructure.Features.Migrations
{
    /// <inheritdoc />
    public partial class Add_Admin_Encounter_Permissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "permissions",
                column: "Code",
                values: new object[]
                {
                    "admin:encounter:unfinalize",
                    "admin:encounter:unlock",
                });

            migrationBuilder.InsertData(
                table: "role_permissions",
                columns: new[] { "PermissionCode", "RoleName" },
                values: new object[,]
                {
                    { "admin:encounter:unfinalize", "Administrator" },
                    { "admin:encounter:unlock", "Administrator" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "admin:encounter:unfinalize", "Administrator" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "admin:encounter:unlock", "Administrator" });

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Code",
                keyValue: "admin:encounter:unfinalize");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Code",
                keyValue: "admin:encounter:unlock");
        }
    }
}
