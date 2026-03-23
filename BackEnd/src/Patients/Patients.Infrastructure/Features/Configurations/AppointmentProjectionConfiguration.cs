using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Patients.Domain.Entities;
using Patients.Domain.Utilities;
using Patients.Infrastructure.Features.ReadModels;

namespace Patients.Infrastructure.Features.Configurations;

internal sealed class AppointmentProjectionConfiguration 
    : IEntityTypeConfiguration<AppointmentProjection>
{
    public void Configure(EntityTypeBuilder<AppointmentProjection> builder)
    {
        builder.ToTable("AppointmentProjections");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasMaxLength(PatientsBusinessConfiguration.ID_MAX_LENGTH)
            .IsRequired();

        builder.Property(x => x.PatientId)
            .HasMaxLength(PatientsBusinessConfiguration.ID_MAX_LENGTH)
            .IsRequired();

        builder.Property(x => x.DoctorId)
            .HasMaxLength(PatientsBusinessConfiguration.ID_MAX_LENGTH)
            .IsRequired();

        builder.Property(x => x.Start)
            .IsRequired();

        builder.Property(x => x.End)
            .IsRequired();

        builder.Property(x => x.Status)
            .IsRequired();

        builder.HasIndex(x => x.PatientId);
        builder.HasIndex(x => x.DoctorId);
    }
}