namespace Ordening.Domain.Models;
public class Order : Aggregate<Guid>
{
    private readonly List<OrderItem> _orderItems = new();
    public IReadOnlyList<OrderItem> OrderItems => _orderItems.AsReadOnly();
    public Guid CustomerId { get; private set; }
    public string OrderName { get; private set; } = default!;
    public Address ShippingAddress { get; private set; } = default!; //Dirección de envío    
    public Address BillingAddress { get; private set; } = default!; //Dirección de envío - facturación
    public Payment Payment { get; private set; } = default!;
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;
    public decimal TotalPrice
    {
        get => _orderItems.Sum(item => item.Price * item.Quantity);
        private set { }
    }
}

