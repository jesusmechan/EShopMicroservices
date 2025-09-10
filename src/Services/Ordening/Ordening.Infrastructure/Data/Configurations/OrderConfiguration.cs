using Mapster;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Identity.Client.AppConfig;
using Ordening.Domain.Enums;

namespace Ordening.Infrastructure.Data.Configurations;
public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasConversion(
                                    id => id.Value,
                                    value => OrderId.Of(value));

        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(x => x.CustomerId)
            .IsRequired();

        builder.HasMany(x => x.OrderItems)
            .WithOne()
            .HasForeignKey(oi => oi.OrderId)
            .IsRequired();

        builder.ComplexProperty(
                x => x.OrderName, nameBuilder =>
                {
                    nameBuilder.Property(y => y.Value)
                    .HasColumnName("OrderName")
                                 .IsRequired()
                                 .HasMaxLength(100);
                }
            );

        builder.ComplexProperty(

                x => x.ShippingAddress, addressBuilder =>
                {
                    addressBuilder.Property(y => y.FirstName)
                        .HasMaxLength(100)
                        .IsRequired();

                    addressBuilder.Property(y => y.LastName)
                        .HasMaxLength(100)
                        .IsRequired();

                    addressBuilder.Property(y => y.EmailAddress)
                        .HasMaxLength(255)
                        .IsRequired();

                    addressBuilder.Property(x => x.AddressLine)
                        .HasMaxLength(255)
                        .IsRequired();

                    addressBuilder.Property(x => x.Country)
                        .HasMaxLength(100);

                    addressBuilder.Property(x => x.State)
                        .HasMaxLength(100);

                    addressBuilder.Property(x => x.ZipCode)
                        .HasMaxLength(5)
                        .IsRequired();
                }
            );


        builder.ComplexProperty(

                x => x.BillingAddress, addressBuilder =>
                {
                    addressBuilder.Property(y => y.FirstName)
                        .HasMaxLength(100)
                        .IsRequired();

                    addressBuilder.Property(y => y.LastName)
                        .HasMaxLength(100)
                        .IsRequired();

                    addressBuilder.Property(y => y.EmailAddress)
                        .HasMaxLength(255)
                        .IsRequired();

                    addressBuilder.Property(x => x.AddressLine)
                        .HasMaxLength(255)
                        .IsRequired();

                    addressBuilder.Property(x => x.Country)
                        .HasMaxLength(100);

                    addressBuilder.Property(x => x.State)
                        .HasMaxLength(100);

                    addressBuilder.Property(x => x.ZipCode)
                        .HasMaxLength(5)
                        .IsRequired();
                }
            );


        builder.ComplexProperty(
             x => x.Payment, paymentBuilder =>
             {

                 paymentBuilder.Property(x => x.CardName)
                        .HasMaxLength(50);

                 paymentBuilder.Property(x => x.CardNumber)
                        .HasMaxLength(24)
                        .IsRequired();

                 paymentBuilder.Property(x => x.Expiration)
                        .HasMaxLength(10)
                        .IsRequired();

                 paymentBuilder.Property(x => x.CVV)
                        .HasMaxLength(3)
                        .IsRequired();

                 paymentBuilder.Property(x => x.PaymentMethod);
             }
         );

        builder.Property(x => x.Status)
            .HasDefaultValue(OrderStatus.Draft)
            .HasConversion(
                y => y.ToString(),
                dbStatus => (OrderStatus)Enum.Parse(typeof(Order), dbStatus)
            );

        builder.Property(x => x.TotalPrice)
            .HasColumnType("decimal(18,2)");
    }
}
