using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Domain.Entities;

namespace Users.Infrastructure.Features.Configurations;

internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");

        builder.HasKey(p => p.Code);

        builder.Property(p => p.Code).HasMaxLength(100);

        builder.HasData(
            // User Permissions
            Permission.GetUser,
            Permission.ModifyUser,
            Permission.DeleteUser,

            // Appointment Permissions
            Permission.CreateAppointment,
            Permission.CancelAppointment,
            Permission.RescheduleAppointment,
            Permission.GetAppointment,

            // Doctor Permissions
            Permission.CreateDoctor,
            Permission.UpdateDoctor,
            Permission.ViewDoctor,
            Permission.CreateDoctorByAdmin,
            Permission.UpdateDoctorByAdmin,
            Permission.ViewDoctorByAdmin,
            Permission.ViewAllDoctors,
            Permission.AddSpeciality,
            Permission.RemoveSpeciality,
            Permission.RequestRecommendations,
            Permission.AddWorkDaySchedule,
            Permission.ChangeWorkDaySchedule,
            Permission.RemoveWorkDaySchedule,
            Permission.AddExtraAvailability,
            Permission.RemoveExtraAvailability,
            Permission.AddUnavailability,
            Permission.RemoveUnavailability,
            
            // Rating Permissions
            Permission.AddRating,
            Permission.RemoveRating,
            Permission.EditRating,
            Permission.GetRating,
            Permission.GetRatingStats
            
        );

        builder
            .HasMany<Role>()
            .WithMany()
            .UsingEntity(joinBuilder =>
            {
                joinBuilder.ToTable("role_permissions");

                joinBuilder.HasData(
                    // --- Admin permissions ---
                    // Admin user permissions
                    CreateRolePermission(Role.Administrator, Permission.GetUser),
                    CreateRolePermission(Role.Administrator, Permission.ModifyUser),
                    CreateRolePermission(Role.Administrator, Permission.DeleteUser),
                    // Admin appointment permissions
                    CreateRolePermission(Role.Administrator, Permission.CreateAppointment),
                    CreateRolePermission(Role.Administrator, Permission.CancelAppointment),
                    CreateRolePermission(Role.Administrator, Permission.RescheduleAppointment),
                    CreateRolePermission(Role.Administrator, Permission.GetAppointment),
                    // Admin doctor permissions
                    CreateRolePermission(Role.Administrator, Permission.CreateDoctor),
                    CreateRolePermission(Role.Administrator, Permission.UpdateDoctor),
                    CreateRolePermission(Role.Administrator, Permission.ViewDoctor),
                    CreateRolePermission(Role.Administrator, Permission.CreateDoctorByAdmin),
                    CreateRolePermission(Role.Administrator, Permission.UpdateDoctorByAdmin),
                    CreateRolePermission(Role.Administrator, Permission.ViewDoctorByAdmin),
                    CreateRolePermission(Role.Administrator, Permission.ViewAllDoctors),
                    CreateRolePermission(Role.Administrator, Permission.AddSpeciality),
                    CreateRolePermission(Role.Administrator, Permission.RemoveSpeciality),
                    CreateRolePermission(Role.Administrator, Permission.RequestRecommendations),
                    CreateRolePermission(Role.Administrator, Permission.AddWorkDaySchedule),
                    CreateRolePermission(Role.Administrator, Permission.ChangeWorkDaySchedule),
                    CreateRolePermission(Role.Administrator, Permission.RemoveWorkDaySchedule),
                    CreateRolePermission(Role.Administrator, Permission.AddExtraAvailability),
                    CreateRolePermission(Role.Administrator, Permission.RemoveExtraAvailability),
                    CreateRolePermission(Role.Administrator, Permission.AddUnavailability),
                    CreateRolePermission(Role.Administrator, Permission.RemoveUnavailability),
                    // Admin rating permissions
                    CreateRolePermission(Role.Administrator, Permission.AddRating),
                    CreateRolePermission(Role.Administrator, Permission.RemoveRating),
                    CreateRolePermission(Role.Administrator, Permission.EditRating),
                    CreateRolePermission(Role.Administrator, Permission.GetRating),
                    CreateRolePermission(Role.Administrator, Permission.GetRatingStats),

                    // --- Doctor permissions ---
                    CreateRolePermission(Role.Doctor, Permission.CreateAppointment),
                    CreateRolePermission(Role.Doctor, Permission.CancelAppointment),
                    CreateRolePermission(Role.Doctor, Permission.RescheduleAppointment),
                    CreateRolePermission(Role.Doctor, Permission.CreateDoctor),
                    CreateRolePermission(Role.Doctor, Permission.UpdateDoctor),
                    CreateRolePermission(Role.Doctor, Permission.ViewDoctor),
                    CreateRolePermission(Role.Doctor, Permission.AddSpeciality),
                    CreateRolePermission(Role.Doctor, Permission.RemoveSpeciality),
                    CreateRolePermission(Role.Doctor, Permission.RequestRecommendations),
                    CreateRolePermission(Role.Doctor, Permission.AddWorkDaySchedule),
                    CreateRolePermission(Role.Doctor, Permission.ChangeWorkDaySchedule),
                    CreateRolePermission(Role.Doctor, Permission.RemoveWorkDaySchedule),
                    CreateRolePermission(Role.Doctor, Permission.AddExtraAvailability),
                    CreateRolePermission(Role.Doctor, Permission.RemoveExtraAvailability),
                    CreateRolePermission(Role.Doctor, Permission.AddUnavailability),
                    CreateRolePermission(Role.Doctor, Permission.RemoveUnavailability),
                    CreateRolePermission(Role.Doctor, Permission.GetRating),
                    CreateRolePermission(Role.Doctor, Permission.GetRatingStats),

                    // --- Patient permissions ---
                    CreateRolePermission(Role.Patient, Permission.CreateAppointment),
                    CreateRolePermission(Role.Patient, Permission.CancelAppointment),
                    CreateRolePermission(Role.Patient, Permission.RescheduleAppointment),
                    CreateRolePermission(Role.Patient, Permission.RequestRecommendations),
                    CreateRolePermission(Role.Patient, Permission.AddRating),
                    CreateRolePermission(Role.Patient, Permission.RemoveRating),
                    CreateRolePermission(Role.Patient, Permission.EditRating),
                    CreateRolePermission(Role.Patient, Permission.GetRating),
                    CreateRolePermission(Role.Patient, Permission.GetRatingStats)
                );
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
