using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ratings.Domain.Entities;

namespace Ratings.Infrastructure.Features.Configurations;

internal sealed class DoctorRatingStatsConfiguration
    : IEntityTypeConfiguration<DoctorRatingStats>
{
    public void Configure(EntityTypeBuilder<DoctorRatingStats> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.AverageRating)
            .IsRequired();

        builder.Property(x => x.RatingsCount)
            .IsRequired();
    }
}
