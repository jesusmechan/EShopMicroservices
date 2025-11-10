
namespace Security.Infrastrucure.Data.Configurations;
public class DocumentTypeConfiguration : IEntityTypeConfiguration<DocumentType>
{
    public void Configure(EntityTypeBuilder<DocumentType> builder)
    {
        builder.Property(x => x.Id)
               .ValueGeneratedOnAdd();

        builder.Property(x => x.Description)
            .IsRequired()
                .HasMaxLength(100);

        builder.Property(x => x.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

    }
}
