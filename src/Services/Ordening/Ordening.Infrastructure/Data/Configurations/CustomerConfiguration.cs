using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ordening.Infrastructure.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
               .HasConversion(
                   id => id.Value,
                   value => CustomerId.Of(value));
        builder.Property(x => x.Name)
               .IsRequired()
               .HasMaxLength(100);
        builder.Property(x => x.Email)
               .IsRequired()
               .HasMaxLength(255);

        builder.HasIndex(x => x.Email)
               .IsUnique();


    }
}
