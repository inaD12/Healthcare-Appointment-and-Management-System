using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Domain.Entities;

namespace Appointments.Infrastructure.Features.Configuration;

internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");

        builder.HasKey(p => p.Code);

        builder.Property(p => p.Code).HasMaxLength(100);

        builder.HasData(
            Permission.GetUser,
            Permission.ModifyUser,
            Permission.DeleteUser,
            Permission.CreateAppointment,
            Permission.CancelAppointment,
            Permission.RescheduleAppointment,
            Permission.GetAppointment);

        builder
            .HasMany<Role>()
            .WithMany()
            .UsingEntity(joinBuilder =>
            {
                joinBuilder.ToTable("role_permissions");

                joinBuilder.HasData(
                    // Admin permissions
                    CreateRolePermission(Role.Administrator, Permission.GetUser),
                    CreateRolePermission(Role.Administrator, Permission.ModifyUser),
                    CreateRolePermission(Role.Administrator, Permission.DeleteUser),
                    CreateRolePermission(Role.Administrator, Permission.CreateAppointment),
                    CreateRolePermission(Role.Administrator, Permission.CancelAppointment),
                    CreateRolePermission(Role.Administrator, Permission.RescheduleAppointment),
                    CreateRolePermission(Role.Administrator, Permission.GetAppointment),
                    // Patient permissions
                    CreateRolePermission(Role.Patient, Permission.GetUser),
                    CreateRolePermission(Role.Patient, Permission.ModifyUser),
                    CreateRolePermission(Role.Patient, Permission.DeleteUser),
                    CreateRolePermission(Role.Patient, Permission.CreateAppointment),
                    CreateRolePermission(Role.Patient, Permission.CancelAppointment),
                    CreateRolePermission(Role.Patient, Permission.RescheduleAppointment),
                    CreateRolePermission(Role.Patient, Permission.GetAppointment),
                    // Doctor permissions
                    CreateRolePermission(Role.Doctor, Permission.GetUser),
                    CreateRolePermission(Role.Doctor, Permission.ModifyUser),
                    CreateRolePermission(Role.Doctor, Permission.DeleteUser),
                    CreateRolePermission(Role.Doctor, Permission.CreateAppointment),
                    CreateRolePermission(Role.Doctor, Permission.CancelAppointment),
                    CreateRolePermission(Role.Doctor, Permission.RescheduleAppointment),
                    CreateRolePermission(Role.Doctor, Permission.GetAppointment));
            });
    }

    private static object CreateRolePermission(Role role, Permission permission)
    {
        return new
        {
            RoleName = role.Name,
            PermissionCode = permission.Code
        };
    }
}
