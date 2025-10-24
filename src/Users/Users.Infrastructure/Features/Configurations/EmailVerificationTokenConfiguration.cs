using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Entities;

namespace Users.Infrastructure.Features.Configurations;

internal class EmailVerificationTokenConfiguration : IEntityTypeConfiguration<EmailVerificationToken>
{
	public void Configure(EntityTypeBuilder<EmailVerificationToken> builder)
	{
		builder.HasKey(e => e.Id);
		builder.Property(e => e.UserId).IsRequired();

		builder.HasOne(e => e.User)
			.WithMany()
			.HasForeignKey(e => e.UserId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
