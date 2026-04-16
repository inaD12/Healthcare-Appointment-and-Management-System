using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Users.Infrastructure.Features.Migrations
{
    /// <inheritdoc />
    public partial class Add_RemoveRatingAdmin_Permission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "permissions",
                column: "Code",
                value: "ratings:admin:delete");

            migrationBuilder.InsertData(
                table: "role_permissions",
                columns: new[] { "PermissionCode", "RoleName" },
                values: new object[] { "ratings:admin:delete", "Administrator" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "role_permissions",
                keyColumns: new[] { "PermissionCode", "RoleName" },
                keyValues: new object[] { "ratings:admin:delete", "Administrator" });

            migrationBuilder.DeleteData(
                table: "permissions",
                keyColumn: "Code",
                keyValue: "ratings:admin:delete");
        }
    }
}
