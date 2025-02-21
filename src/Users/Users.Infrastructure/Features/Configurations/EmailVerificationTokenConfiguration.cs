using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Users.Domain.Entities;

namespace Users.Infrastructure.Features.Configurations;

internal class EmailVerificationTokenConfiguration : IEntityTypeConfiguration<EmailVerificationToken>
{
	public void Configure(EntityTypeBuilder<EmailVerificationToken> builder)
	{
		builder.HasKey(x => x.Id);
		builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId);
	}
}
