using Ordening.Domain.Abstractions;

namespace Ordening.Domain.Models;

public class OrderItem : Entity<Guid>
{
    public Guid OrderId { get; private set; } = default!;
    public Guid ProductId { get; private set; } = default!;
    public int Quantity { get; private set; } = default!;
    public decimal Price { get; private set; } = default!;

    public OrderItem(Guid orderId, Guid productId, int quantity, decimal price)
    {
        Id = Guid.NewGuid();
        OrderId = orderId;
        ProductId = productId;
        Quantity = quantity;
        Price = price;
    }
}
