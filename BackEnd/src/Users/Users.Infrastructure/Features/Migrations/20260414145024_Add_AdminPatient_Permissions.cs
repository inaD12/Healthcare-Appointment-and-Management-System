using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Users.Infrastructure.Features.Migrations
{
    /// <inheritdoc />
    public partial class Add_AdminPatient_Permissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "permissions",
                column: "Code",
                values: new object[]
                {
                    "patient:admin:allergy:add",
                    "patient:admin:allergy:remove",
                    "patient:admin:condition:add",
                    "patient:admin:condition:remove"
                });

            migrationBuilder.InsertData(
                table: "role_permissions",
                columns: new[] { "PermissionCode", "RoleName" },
                values: new object[,]
                {
                    { "patient:admin:allergy:add", "Administrator" },
                    { "patient:admin:allergy:remove", "Administrator" },
                    { "patient:admin:condition:add", "Administrator" },
                    { "patient:admin:condition:remove", "Administrator" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "patient:admin:allergy:add", "Administrator" });

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Code",
                keyValue: "patient:admin:allergy:add");
        }
    }
}
