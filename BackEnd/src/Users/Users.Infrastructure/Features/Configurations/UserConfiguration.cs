using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Entities;

namespace Users.Infrastructure.Features.Configurations;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
	public void Configure(EntityTypeBuilder<User> builder)
	{
		builder.HasKey(e => e.Id);
		builder.HasIndex(e => e.Email).IsUnique();
		builder.HasIndex(e => e.IdentityId).IsUnique();
	}
}
