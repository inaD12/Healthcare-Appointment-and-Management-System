using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Users.Infrastructure.Features.Migrations
{
    /// <inheritdoc />
    public partial class Add_Database : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InboxState",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MessageId = table.Column<Guid>(type: "uuid", nullable: false),
                    ConsumerId = table.Column<Guid>(type: "uuid", nullable: false),
                    LockId = table.Column<Guid>(type: "uuid", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    Received = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReceiveCount = table.Column<int>(type: "integer", nullable: false),
                    ExpirationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Consumed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Delivered = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastSequenceNumber = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboxState", x => x.Id);
                    table.UniqueConstraint("AK_InboxState_MessageId_ConsumerId", x => new { x.MessageId, x.ConsumerId });
                });

            migrationBuilder.CreateTable(
                name: "OutboxState",
                columns: table => new
                {
                    OutboxId = table.Column<Guid>(type: "uuid", nullable: false),
                    LockId = table.Column<Guid>(type: "uuid", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Delivered = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastSequenceNumber = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxState", x => x.OutboxId);
                });

            migrationBuilder.CreateTable(
                name: "permissions",
                columns: table => new
                {
                    Code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permissions", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "text", nullable: false),
                    EmailVerified = table.Column<bool>(type: "boolean", nullable: false),
                    IdentityId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OutboxMessage",
                columns: table => new
                {
                    SequenceNumber = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EnqueueTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    SentTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Headers = table.Column<string>(type: "text", nullable: true),
                    Properties = table.Column<string>(type: "text", nullable: true),
                    InboxMessageId = table.Column<Guid>(type: "uuid", nullable: true),
                    InboxConsumerId = table.Column<Guid>(type: "uuid", nullable: true),
                    OutboxId = table.Column<Guid>(type: "uuid", nullable: true),
                    MessageId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContentType = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    MessageType = table.Column<string>(type: "text", nullable: false),
                    Body = table.Column<string>(type: "text", nullable: false),
                    ConversationId = table.Column<Guid>(type: "uuid", nullable: true),
                    CorrelationId = table.Column<Guid>(type: "uuid", nullable: true),
                    InitiatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    RequestId = table.Column<Guid>(type: "uuid", nullable: true),
                    SourceAddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    DestinationAddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ResponseAddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    FaultAddress = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    ExpirationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboxMessage", x => x.SequenceNumber);
                    table.ForeignKey(
                        name: "FK_OutboxMessage_InboxState_InboxMessageId_InboxConsumerId",
                        columns: x => new { x.InboxMessageId, x.InboxConsumerId },
                        principalTable: "InboxState",
                        principalColumns: new[] { "MessageId", "ConsumerId" });
                    table.ForeignKey(
                        name: "FK_OutboxMessage_OutboxState_OutboxId",
                        column: x => x.OutboxId,
                        principalTable: "OutboxState",
                        principalColumn: "OutboxId");
                });

            migrationBuilder.CreateTable(
                name: "role_permissions",
                columns: table => new
                {
                    PermissionCode = table.Column<string>(type: "character varying(100)", nullable: false),
                    RoleName = table.Column<string>(type: "character varying(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_role_permissions", x => new { x.PermissionCode, x.RoleName });
                    table.ForeignKey(
                        name: "FK_role_permissions_permissions_PermissionCode",
                        column: x => x.PermissionCode,
                        principalTable: "permissions",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_role_permissions_roles_RoleName",
                        column: x => x.RoleName,
                        principalTable: "roles",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmailVerificationTokens",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiresOnUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailVerificationTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailVerificationTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    role_name = table.Column<string>(type: "character varying(50)", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_roles", x => new { x.role_name, x.UserId });
                    table.ForeignKey(
                        name: "FK_user_roles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_roles_roles_role_name",
                        column: x => x.role_name,
                        principalTable: "roles",
                        principalColumn: "Name",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "permissions",
                column: "Code",
                values: new object[]
                {
                    "appointment:cancel",
                    "appointment:create",
                    "appointment:read",
                    "appointment:reschedule",
                    "doctor:admin:create",
                    "doctor:admin:update",
                    "doctor:admin:view",
                    "doctor:availability:extra:add",
                    "doctor:availability:extra:remove",
                    "doctor:availability:unavailable:add",
                    "doctor:availability:unavailable:remove",
                    "doctor:create",
                    "doctor:schedule:workday:add",
                    "doctor:schedule:workday:remove",
                    "doctor:schedule:workday:update",
                    "doctor:speciality:add",
                    "doctor:speciality:recommend",
                    "doctor:speciality:remove",
                    "doctor:update",
                    "doctor:view",
                    "doctor:view-all",
                    "encounter:addendum:add",
                    "encounter:addendum:view",
                    "encounter:diagnosis:add",
                    "encounter:diagnosis:remove",
                    "encounter:diagnosis:view",
                    "encounter:edit",
                    "encounter:finalize",
                    "encounter:lock",
                    "encounter:note:add",
                    "encounter:note:remove",
                    "encounter:note:view",
                    "encounter:prescription:add",
                    "encounter:prescription:remove",
                    "encounter:prescription:view",
                    "encounter:start",
                    "encounter:view",
                    "patient:admin:delete",
                    "patient:admin:view-all",
                    "patient:allergy:add",
                    "patient:allergy:remove",
                    "patient:allergy:view",
                    "patient:condition:add",
                    "patient:condition:remove",
                    "patient:condition:view",
                    "patient:create",
                    "patient:delete",
                    "patient:update",
                    "patient:view",
                    "ratings:create",
                    "ratings:delete",
                    "ratings:read",
                    "ratings:update",
                    "ratingStats:read",
                    "users:delete",
                    "users:read",
                    "users:update"
                });

            migrationBuilder.InsertData(
                table: "roles",
                column: "Name",
                values: new object[]
                {
                    "Administrator",
                    "Doctor",
                    "Patient"
                });

            migrationBuilder.InsertData(
                table: "role_permissions",
                columns: new[] { "PermissionCode", "RoleName" },
                values: new object[,]
                {
                    { "appointment:cancel", "Administrator" },
                    { "appointment:cancel", "Doctor" },
                    { "appointment:cancel", "Patient" },
                    { "appointment:create", "Administrator" },
                    { "appointment:create", "Doctor" },
                    { "appointment:create", "Patient" },
                    { "appointment:read", "Administrator" },
                    { "appointment:reschedule", "Administrator" },
                    { "appointment:reschedule", "Doctor" },
                    { "appointment:reschedule", "Patient" },
                    { "doctor:admin:create", "Administrator" },
                    { "doctor:admin:update", "Administrator" },
                    { "doctor:admin:view", "Administrator" },
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
                    { "doctor:speciality:recommend", "Administrator" },
                    { "doctor:speciality:recommend", "Doctor" },
                    { "doctor:speciality:recommend", "Patient" },
                    { "doctor:speciality:remove", "Administrator" },
                    { "doctor:speciality:remove", "Doctor" },
                    { "doctor:update", "Administrator" },
                    { "doctor:update", "Doctor" },
                    { "doctor:view", "Administrator" },
                    { "doctor:view", "Doctor" },
                    { "doctor:view-all", "Administrator" },
                    { "doctor:view-all", "Doctor" },
                    { "doctor:view-all", "Patient" },
                    { "encounter:addendum:add", "Administrator" },
                    { "encounter:addendum:add", "Doctor" },
                    { "encounter:addendum:view", "Administrator" },
                    { "encounter:addendum:view", "Doctor" },
                    { "encounter:addendum:view", "Patient" },
                    { "encounter:diagnosis:add", "Administrator" },
                    { "encounter:diagnosis:add", "Doctor" },
                    { "encounter:diagnosis:remove", "Administrator" },
                    { "encounter:diagnosis:remove", "Doctor" },
                    { "encounter:diagnosis:view", "Administrator" },
                    { "encounter:diagnosis:view", "Doctor" },
                    { "encounter:diagnosis:view", "Patient" },
                    { "encounter:edit", "Administrator" },
                    { "encounter:edit", "Doctor" },
                    { "encounter:finalize", "Administrator" },
                    { "encounter:finalize", "Doctor" },
                    { "encounter:lock", "Administrator" },
                    { "encounter:lock", "Doctor" },
                    { "encounter:note:add", "Administrator" },
                    { "encounter:note:add", "Doctor" },
                    { "encounter:note:remove", "Administrator" },
                    { "encounter:note:remove", "Doctor" },
                    { "encounter:note:view", "Administrator" },
                    { "encounter:note:view", "Doctor" },
                    { "encounter:note:view", "Patient" },
                    { "encounter:prescription:add", "Administrator" },
                    { "encounter:prescription:add", "Doctor" },
                    { "encounter:prescription:remove", "Administrator" },
                    { "encounter:prescription:remove", "Doctor" },
                    { "encounter:prescription:view", "Administrator" },
                    { "encounter:prescription:view", "Doctor" },
                    { "encounter:prescription:view", "Patient" },
                    { "encounter:start", "Administrator" },
                    { "encounter:start", "Doctor" },
                    { "encounter:view", "Administrator" },
                    { "encounter:view", "Doctor" },
                    { "encounter:view", "Patient" },
                    { "patient:admin:delete", "Administrator" },
                    { "patient:admin:view-all", "Administrator" },
                    { "patient:allergy:add", "Administrator" },
                    { "patient:allergy:remove", "Administrator" },
                    { "patient:allergy:view", "Administrator" },
                    { "patient:allergy:view", "Doctor" },
                    { "patient:allergy:view", "Patient" },
                    { "patient:condition:add", "Administrator" },
                    { "patient:condition:remove", "Administrator" },
                    { "patient:condition:view", "Administrator" },
                    { "patient:condition:view", "Doctor" },
                    { "patient:condition:view", "Patient" },
                    { "patient:create", "Administrator" },
                    { "patient:delete", "Administrator" },
                    { "patient:update", "Administrator" },
                    { "patient:view", "Administrator" },
                    { "patient:view", "Doctor" },
                    { "patient:view", "Patient" },
                    { "ratings:create", "Administrator" },
                    { "ratings:create", "Patient" },
                    { "ratings:delete", "Administrator" },
                    { "ratings:delete", "Patient" },
                    { "ratings:read", "Administrator" },
                    { "ratings:read", "Doctor" },
                    { "ratings:read", "Patient" },
                    { "ratings:update", "Administrator" },
                    { "ratings:update", "Patient" },
                    { "ratingStats:read", "Administrator" },
                    { "ratingStats:read", "Doctor" },
                    { "ratingStats:read", "Patient" },
                    { "users:delete", "Administrator" },
                    { "users:read", "Administrator" },
                    { "users:update", "Administrator" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailVerificationTokens_UserId",
                table: "EmailVerificationTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_InboxState_Delivered",
                table: "InboxState",
                column: "Delivered");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_EnqueueTime",
                table: "OutboxMessage",
                column: "EnqueueTime");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_ExpirationTime",
                table: "OutboxMessage",
                column: "ExpirationTime");

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_InboxMessageId_InboxConsumerId_SequenceNumber",
                table: "OutboxMessage",
                columns: new[] { "InboxMessageId", "InboxConsumerId", "SequenceNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OutboxMessage_OutboxId_SequenceNumber",
                table: "OutboxMessage",
                columns: new[] { "OutboxId", "SequenceNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OutboxState_Created",
                table: "OutboxState",
                column: "Created");

            migrationBuilder.CreateIndex(
                name: "IX_role_permissions_RoleName",
                table: "role_permissions",
                column: "RoleName");

            migrationBuilder.CreateIndex(
                name: "IX_user_roles_UserId",
                table: "user_roles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_IdentityId",
                table: "Users",
                column: "IdentityId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailVerificationTokens");

            migrationBuilder.DropTable(
                name: "OutboxMessage");

            migrationBuilder.DropTable(
                name: "role_permissions");

            migrationBuilder.DropTable(
                name: "user_roles");

            migrationBuilder.DropTable(
                name: "InboxState");

            migrationBuilder.DropTable(
                name: "OutboxState");

            migrationBuilder.DropTable(
                name: "permissions");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "roles");
        }
    }
}
