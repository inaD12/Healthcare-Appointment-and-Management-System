using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Users.Infrastructure.Features.Migrations
{
    /// <inheritdoc />
    public partial class Remove_Some_Admin_Permissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "doctor:availability:extra:add", "Administrator" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "doctor:availability:extra:remove", "Administrator" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "doctor:availability:unavailable:add", "Administrator" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "doctor:availability:unavailable:remove", "Administrator" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "doctor:schedule:workday:add", "Administrator" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "doctor:schedule:workday:remove", "Administrator" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "doctor:schedule:workday:update", "Administrator" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "encounter:addendum:add", "Administrator" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "encounter:diagnosis:add", "Administrator" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "encounter:diagnosis:remove", "Administrator" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "encounter:edit", "Administrator" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "encounter:finalize", "Administrator" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "encounter:lock", "Administrator" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "encounter:note:add", "Administrator" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "encounter:note:remove", "Administrator" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "encounter:prescription:add", "Administrator" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "encounter:prescription:remove", "Administrator" });

            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "encounter:start", "Administrator" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "role_permissions",
                columns: new[] { "PermissionCode", "RoleName" },
                values: new object[,]
                {
                    { "doctor:availability:extra:add", "Administrator" },
                    { "doctor:availability:extra:remove", "Administrator" },
                    { "doctor:availability:unavailable:add", "Administrator" },
                    { "doctor:availability:unavailable:remove", "Administrator" },
                    { "doctor:schedule:workday:add", "Administrator" },
                    { "doctor:schedule:workday:remove", "Administrator" },
                    { "doctor:schedule:workday:update", "Administrator" },
                    { "encounter:addendum:add", "Administrator" },
                    { "encounter:diagnosis:add", "Administrator" },
                    { "encounter:diagnosis:remove", "Administrator" },
                    { "encounter:edit", "Administrator" },
                    { "encounter:finalize", "Administrator" },
                    { "encounter:lock", "Administrator" },
                    { "encounter:note:add", "Administrator" },
                    { "encounter:note:remove", "Administrator" },
                    { "encounter:prescription:add", "Administrator" },
                    { "encounter:prescription:remove", "Administrator" },
                    { "encounter:start", "Administrator" }
                });
        }
    }
}
