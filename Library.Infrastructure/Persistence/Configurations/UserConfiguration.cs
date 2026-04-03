namespace Library.Infrastructure.Persistence.Configurations
{
	public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.OwnsOne(u => u.Email, email =>
			{
				email.Property(e => e.Value)
					.HasColumnName("Email")
					.IsRequired()
					.HasMaxLength(255);

				email.HasIndex(e => e.Value)
					.IsUnique();
			});

			builder.OwnsOne(u => u.FullName, fullName =>
			{
				fullName.Property(f => f.FirstName)
					.HasColumnName("FirstName")
					.IsRequired()
					.HasMaxLength(100);

				fullName.Property(f => f.LastName)
					.HasColumnName("LastName")
					.IsRequired()
					.HasMaxLength(100);
			});

			builder.OwnsOne(u => u.ContactInfo, contactInfo =>
			{
				contactInfo.Property(c => c.Address)
					.HasColumnName("Address")
					.HasMaxLength(500);

				contactInfo.Property(c => c.PhoneNumber)
					.HasColumnName("PhoneNumber")
					.HasMaxLength(20);
			});

			builder.Property(u => u.PasswordSalt)
				.IsRequired();

			builder.Property(u => u.PasswordHash)
				.IsRequired();

			builder.Property(u => u.Role)
				.IsRequired()
				.HasConversion<string>();

			builder.HasMany(u => u.Borrowings)
				.WithOne(b => b.User)
				.HasForeignKey(b => b.UserId);
		}
	}
}
