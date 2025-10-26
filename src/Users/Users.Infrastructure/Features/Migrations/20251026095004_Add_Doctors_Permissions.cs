using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Users.Infrastructure.Features.Migrations
{
    /// <inheritdoc />
    public partial class Add_Doctors_Permissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "permissions",
                column: "Code",
                values: new object[]
                {
                    "doctor:admin:create",
                    "doctor:admin:update",
                    "doctor:admin:view",
                    "doctor:admin:view-all",
                    "doctor:availability:extra:add",
                    "doctor:availability:extra:remove",
                    "doctor:availability:unavailable:add",
                    "doctor:availability:unavailable:remove",
                    "doctor:create",
                    "doctor:schedule:workday:add",
                    "doctor:schedule:workday:remove",
                    "doctor:schedule:workday:update",
                    "doctor:speciality:add",
                    "doctor:speciality:remove",
                    "doctor:update",
                    "doctor:view"
                });

            migrationBuilder.InsertData(
                table: "role_permissions",
                columns: new[] { "PermissionCode", "RoleName" },
                values: new object[,]
                {
                    { "doctor:admin:create", "Administrator" },
                    { "doctor:admin:update", "Administrator" },
                    { "doctor:admin:view", "Administrator" },
                    { "doctor:admin:view-all", "Administrator" },
                    { "doctor:availability:extra:add", "Administrator" },
                    { "doctor:availability:extra:add", "Doctor" },
                    { "doctor:availability:extra:remove", "Administrator" },
                    { "doctor:availability:extra:remove", "Doctor" },
                    { "doctor:availability:unavailable:add", "Administrator" },
                    { "doctor:availability:unavailable:add", "Doctor" },
                    { "doctor:availability:unavailable:remove", "Administrator" },
                    { "doctor:availability:unavailable:remove", "Doctor" },
                    { "doctor:create", "Administrator" },
                    { "doctor:create", "Doctor" },
                    { "doctor:schedule:workday:add", "Administrator" },
                    { "doctor:schedule:workday:add", "Doctor" },
                    { "doctor:schedule:workday:remove", "Administrator" },
                    { "doctor:schedule:workday:remove", "Doctor" },
                    { "doctor:schedule:workday:update", "Administrator" },
                    { "doctor:schedule:workday:update", "Doctor" },
                    { "doctor:speciality:add", "Administrator" },
                    { "doctor:speciality:add", "Doctor" },
                    { "doctor:speciality:remove", "Administrator" },
                    { "doctor:speciality:remove", "Doctor" },
                    { "doctor:update", "Administrator" },
                    { "doctor:update", "Doctor" },
                    { "doctor:view", "Administrator" },
                    { "doctor:view", "Doctor" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "doctor:admin:create", "Administrator" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "doctor:admin:update", "Administrator" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "doctor:admin:view", "Administrator" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "doctor:admin:view-all", "Administrator" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "doctor:availability:extra:add", "Administrator" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "doctor:availability:extra:add", "Doctor" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "doctor:availability:extra:remove", "Administrator" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "doctor:availability:extra:remove", "Doctor" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "doctor:availability:unavailable:add", "Administrator" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "doctor:availability:unavailable:add", "Doctor" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "doctor:availability:unavailable:remove", "Administrator" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "doctor:availability:unavailable:remove", "Doctor" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "doctor:create", "Administrator" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "doctor:create", "Doctor" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "doctor:schedule:workday:add", "Administrator" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "doctor:schedule:workday:add", "Doctor" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "doctor:schedule:workday:remove", "Administrator" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "doctor:schedule:workday:remove", "Doctor" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "doctor:schedule:workday:update", "Administrator" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "doctor:schedule:workday:update", "Doctor" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "doctor:speciality:add", "Administrator" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "doctor:speciality:add", "Doctor" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "doctor:speciality:remove", "Administrator" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "doctor:speciality:remove", "Doctor" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "doctor:update", "Administrator" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "doctor:update", "Doctor" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "doctor:view", "Administrator" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "doctor:view", "Doctor" });

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Code",
                keyValue: "doctor:admin:create");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Code",
                keyValue: "doctor:admin:update");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Code",
                keyValue: "doctor:admin:view");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Code",
                keyValue: "doctor:admin:view-all");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Code",
                keyValue: "doctor:availability:extra:add");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Code",
                keyValue: "doctor:availability:extra:remove");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Code",
                keyValue: "doctor:availability:unavailable:add");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Code",
                keyValue: "doctor:availability:unavailable:remove");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Code",
                keyValue: "doctor:create");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Code",
                keyValue: "doctor:schedule:workday:add");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Code",
                keyValue: "doctor:schedule:workday:remove");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Code",
                keyValue: "doctor:schedule:workday:update");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Code",
                keyValue: "doctor:speciality:add");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Code",
                keyValue: "doctor:speciality:remove");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Code",
                keyValue: "doctor:update");

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Code",
                keyValue: "doctor:view");
        }
    }
}
