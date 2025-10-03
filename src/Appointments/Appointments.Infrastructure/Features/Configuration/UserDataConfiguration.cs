using Appointments.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Shared.Domain.Entities;

namespace Appointments.Infrastructure.Features.Configuration;

internal sealed class UserDataConfiguration : IEntityTypeConfiguration<UserData>
{
    public void Configure(EntityTypeBuilder<UserData> builder)
    {
        builder.HasIndex(u => u.Id).IsUnique();
    }
}
