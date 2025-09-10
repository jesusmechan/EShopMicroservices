using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ordening.Infrastructure.Data.Configurations;
public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id)
               .HasConversion(
                   id => id.Value,
                   value => OrderItemId.Of(value));

        builder.HasOne<Product>()
               .WithMany()
               .HasForeignKey(x => x.ProductId);

        builder.Property(x => x.Quantity)
               .IsRequired();

        builder.Property(x => x.Price)
            .IsRequired()
            .HasColumnType("decimal(18,2)");


    }
}
