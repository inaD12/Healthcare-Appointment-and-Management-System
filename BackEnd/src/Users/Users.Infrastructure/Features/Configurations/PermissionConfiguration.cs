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
            Permission.GetBookings,

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
            Permission.GetRatingStats,
            
            // Patient Permissions
            Permission.CreatePatient,
            Permission.UpdatePatient,
            Permission.ViewPatient,
            Permission.DeletePatient,
            Permission.ViewAllPatients,
            Permission.DeletePatientByAdmin,
            Permission.AddAllergy,
            Permission.RemoveAllergy,
            Permission.ViewAllergies,
            Permission.AddChronicCondition,
            Permission.RemoveChronicCondition,
            Permission.ViewChronicConditions,

            // Encounter Permissions
            Permission.StartEncounter,
            Permission.ViewEncounter,
            Permission.EditEncounter,
            Permission.LockEncounter,
            Permission.FinalizeEncounter,
            Permission.AddNote,
            Permission.RemoveNote,
            Permission.ViewNotes,
            Permission.AddDiagnosis,
            Permission.RemoveDiagnosis,
            Permission.ViewDiagnoses,
            Permission.AddPrescription,
            Permission.RemovePrescription,
            Permission.ViewPrescriptions,
            Permission.AddAddendum,
            Permission.ViewAddendums
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
                    CreateRolePermission(Role.Administrator, Permission.GetBookings),
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
                    // Admin patient permissions
                    CreateRolePermission(Role.Administrator, Permission.CreatePatient),
                    CreateRolePermission(Role.Administrator, Permission.UpdatePatient),
                    CreateRolePermission(Role.Administrator, Permission.ViewPatient),
                    CreateRolePermission(Role.Administrator, Permission.DeletePatient),
                    CreateRolePermission(Role.Administrator, Permission.ViewAllPatients),
                    CreateRolePermission(Role.Administrator, Permission.DeletePatientByAdmin),
                    CreateRolePermission(Role.Administrator, Permission.AddAllergy),
                    CreateRolePermission(Role.Administrator, Permission.RemoveAllergy),
                    CreateRolePermission(Role.Administrator, Permission.ViewAllergies),
                    CreateRolePermission(Role.Administrator, Permission.AddChronicCondition),
                    CreateRolePermission(Role.Administrator, Permission.RemoveChronicCondition),
                    CreateRolePermission(Role.Administrator, Permission.ViewChronicConditions),
                    // Admin encounter permissions
                    CreateRolePermission(Role.Administrator, Permission.StartEncounter),
                    CreateRolePermission(Role.Administrator, Permission.ViewEncounter),
                    CreateRolePermission(Role.Administrator, Permission.EditEncounter),
                    CreateRolePermission(Role.Administrator, Permission.LockEncounter),
                    CreateRolePermission(Role.Administrator, Permission.FinalizeEncounter),
                    CreateRolePermission(Role.Administrator, Permission.AddNote),
                    CreateRolePermission(Role.Administrator, Permission.RemoveNote),
                    CreateRolePermission(Role.Administrator, Permission.ViewNotes),
                    CreateRolePermission(Role.Administrator, Permission.AddDiagnosis),
                    CreateRolePermission(Role.Administrator, Permission.RemoveDiagnosis),
                    CreateRolePermission(Role.Administrator, Permission.ViewDiagnoses),
                    CreateRolePermission(Role.Administrator, Permission.AddPrescription),
                    CreateRolePermission(Role.Administrator, Permission.RemovePrescription),
                    CreateRolePermission(Role.Administrator, Permission.ViewPrescriptions),
                    CreateRolePermission(Role.Administrator, Permission.AddAddendum),
                    CreateRolePermission(Role.Administrator, Permission.ViewAddendums),


                    // --- Doctor permissions ---
                    CreateRolePermission(Role.Doctor, Permission.GetBookings),
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
                    CreateRolePermission(Role.Doctor, Permission.ViewPatient),
                    CreateRolePermission(Role.Doctor, Permission.ViewAllergies),
                    CreateRolePermission(Role.Doctor, Permission.ViewChronicConditions),
                    CreateRolePermission(Role.Doctor, Permission.StartEncounter),
                    CreateRolePermission(Role.Doctor, Permission.ViewEncounter),
                    CreateRolePermission(Role.Doctor, Permission.EditEncounter),
                    CreateRolePermission(Role.Doctor, Permission.LockEncounter),
                    CreateRolePermission(Role.Doctor, Permission.FinalizeEncounter),
                    CreateRolePermission(Role.Doctor, Permission.AddNote),
                    CreateRolePermission(Role.Doctor, Permission.RemoveNote),
                    CreateRolePermission(Role.Doctor, Permission.ViewNotes),
                    CreateRolePermission(Role.Doctor, Permission.AddDiagnosis),
                    CreateRolePermission(Role.Doctor, Permission.RemoveDiagnosis),
                    CreateRolePermission(Role.Doctor, Permission.ViewDiagnoses),
                    CreateRolePermission(Role.Doctor, Permission.AddPrescription),
                    CreateRolePermission(Role.Doctor, Permission.RemovePrescription),
                    CreateRolePermission(Role.Doctor, Permission.ViewPrescriptions),
                    CreateRolePermission(Role.Doctor, Permission.AddAddendum),
                    CreateRolePermission(Role.Doctor, Permission.ViewAddendums),
                    CreateRolePermission(Role.Doctor, Permission.ViewAllDoctors),
                    CreateRolePermission(Role.Doctor, Permission.AddChronicCondition),
                    CreateRolePermission(Role.Doctor, Permission.AddAllergy),


                    // --- Patient permissions ---
                    CreateRolePermission(Role.Patient, Permission.GetBookings),
                    CreateRolePermission(Role.Patient, Permission.CreateAppointment),
                    CreateRolePermission(Role.Patient, Permission.CancelAppointment),
                    CreateRolePermission(Role.Patient, Permission.RescheduleAppointment),
                    CreateRolePermission(Role.Patient, Permission.RequestRecommendations),
                    CreateRolePermission(Role.Patient, Permission.AddRating),
                    CreateRolePermission(Role.Patient, Permission.RemoveRating),
                    CreateRolePermission(Role.Patient, Permission.EditRating),
                    CreateRolePermission(Role.Patient, Permission.GetRating),
                    CreateRolePermission(Role.Patient, Permission.GetRatingStats),
                    CreateRolePermission(Role.Patient, Permission.ViewPatient),
                    CreateRolePermission(Role.Patient, Permission.ViewAllergies),
                    CreateRolePermission(Role.Patient, Permission.ViewChronicConditions),
                    CreateRolePermission(Role.Patient, Permission.ViewEncounter),
                    CreateRolePermission(Role.Patient, Permission.ViewNotes),
                    CreateRolePermission(Role.Patient, Permission.ViewAllDoctors),
                    CreateRolePermission(Role.Patient, Permission.ViewDiagnoses),
                    CreateRolePermission(Role.Patient, Permission.ViewPrescriptions),
                    CreateRolePermission(Role.Patient, Permission.ViewAddendums),
                    CreateRolePermission(Role.Patient, Permission.AddChronicCondition),
                    CreateRolePermission(Role.Patient, Permission.AddAllergy)
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
