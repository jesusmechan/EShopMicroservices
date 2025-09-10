using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordening.Infrastructure.Data.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => ProductId.Of(value)
            );
         builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(100);

        builder.Property(x => x.Price)
               .IsRequired()
               .HasColumnType("decimal(18,2)");
    }   
}
