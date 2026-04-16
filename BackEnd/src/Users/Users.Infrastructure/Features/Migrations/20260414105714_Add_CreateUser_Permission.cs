using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Users.Infrastructure.Features.Migrations
{
    public partial class Add_CreateUser_Permission : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "permissions",
                column: "Code",
                value: "users:create");

            migrationBuilder.InsertData(
                table: "role_permissions",
                columns: new[] { "PermissionCode", "RoleName" },
                values: new object[] { "users:create", "Administrator" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "users:create", "Administrator" });

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Code",
                keyValue: "users:create");
        }
    }
}