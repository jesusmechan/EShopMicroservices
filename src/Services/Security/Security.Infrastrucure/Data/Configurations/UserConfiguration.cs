namespace Security.Infrastructure.Data.Configurations;

public class UserConfiguration
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
               .ValueGeneratedOnAdd();

        builder.Property(x => x.Username)
               .IsRequired()
               .HasMaxLength(255);

        builder.HasIndex(x => x.Username)
               .IsUnique();

        builder.Property(x => x.PasswordHash)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(x => x.Salt)
               .HasMaxLength(255);

        builder.Property(x => x.LastAccessDate)
            .IsRequired(false);

        builder.Property(x => x.IsActive)
               .IsRequired()
               .HasDefaultValue(true);

        builder.Property(x => x.PersonId).IsRequired();

        builder.HasOne<Person>()
                .WithOne()
                .HasForeignKey<User>(x => x.PersonId)
                .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne<Profile>()
        .WithOne()
        .HasForeignKey<User>(x => x.PersonId)
        .OnDelete(DeleteBehavior.Restrict);
    }
}
