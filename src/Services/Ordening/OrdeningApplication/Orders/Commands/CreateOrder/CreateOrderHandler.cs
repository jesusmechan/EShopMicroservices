namespace Ordening.Application.Orders.Commands.CreateOrder;
public class CreateOrderHandler (IApplicationDbContext dbContext)
    : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        //Create order entity form comand object.
        var order = CreateNewOrder(command.Order);

        //Save to database
        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync(cancellationToken);

        //return result
        return new CreateOrderResult(order.Id.Value);
    }

    private Order CreateNewOrder(OrderDto orderDto)
    {
        var shippingAddress = Address.Of
            (
                orderDto.ShippingAddress.FirstName,
                orderDto.ShippingAddress.LastName,
                orderDto.ShippingAddress.EmailAddress,
                orderDto.ShippingAddress.AddressLine,
                orderDto.ShippingAddress.Country,
                orderDto.ShippingAddress.State,
                orderDto.ShippingAddress.ZipCode
            );

           var billingAddres = Address.Of
            (
                orderDto.ShippingAddress.FirstName,
                orderDto.ShippingAddress.LastName,
                orderDto.ShippingAddress.EmailAddress,
                orderDto.ShippingAddress.AddressLine,
                orderDto.ShippingAddress.Country,
                orderDto.ShippingAddress.State,
                orderDto.ShippingAddress.ZipCode
            );

        var newOrder = Order.Create
            (
                id: OrderId.Of(Guid.NewGuid()),
                customerId: CustomerId.Of(orderDto.CustomerId),
                orderName: OrderName.Of(orderDto.OrderName),
                shippingAddress: shippingAddress,
                billingAddress: billingAddres,
                payment: Payment.Of
                    (
                        orderDto.Payment.CardName,
                        orderDto.Payment.CardNumber,
                        orderDto.Payment.Expiration,
                        orderDto.Payment.Cvv,
                        orderDto.Payment.PaymentMethod
                    )
            );

        foreach (var item in orderDto.OrderItems)
        {
            newOrder.Add
                (
                    ProductId.Of(item.ProdcutId),
                    item.Quantity,
                    item.Price
                );
        }

        return newOrder;
    }

}
