namespace Security.Infrastruc.Data.Configurations;
public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
               .ValueGeneratedOnAdd();

        builder.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(250);

        builder.Property(x => x.Description)
               .HasMaxLength(250);

        builder.Property(x => x.IsActive)
               .IsRequired()
               .HasDefaultValue(true);

        builder.ToTable("Roles");
    }
}
