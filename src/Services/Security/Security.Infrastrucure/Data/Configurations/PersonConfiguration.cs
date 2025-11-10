namespace Security.Infrastrucure.Data.Configurations;
public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
       .ValueGeneratedOnAdd();

        builder.Property(x => x.DocumentType)
               .IsRequired();

        builder.Property(x => x.DocumentNumber)
               .IsRequired()
               .HasMaxLength(20);

        builder.Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Phone)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Email)
            .IsRequired()
            .HasMaxLength(100);

        //builder.Property(x => x.BirthDate)
        //    .IsRequired()
        //    .date

        builder.Property(x => x.Address)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

    }
}
