using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Users.Infrastructure.Features.Migrations
{
    /// <inheritdoc />
    public partial class Add_Permissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "role_permissions",
                columns: new[] { "PermissionCode", "RoleName" },
                values: new object[,]
                {
                    { "patient:allergy:add", "Doctor" },
                    { "patient:allergy:add", "Patient" },
                    { "patient:condition:add", "Doctor" },
                    { "patient:condition:add", "Patient" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "patient:allergy:add", "Doctor" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "patient:allergy:add", "Patient" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "patient:condition:add", "Doctor" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "patient:condition:add", "Patient" });
        }
    }
}
