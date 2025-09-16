

namespace Ordening.Application.Orders.Commands.UpdateOrder;
public class UpdateOrderHandler(IApplicationDbContext dbContext) : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
{
    public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        //UpdateOrder Order entity from command object.
        //save to database
        //return result
        //var orderId = OrderId.Of(command.Order.Id);
        //var order = await dbContext.Orders.FindAsync([orderId], cancellationToken);
        var orderId = OrderId.Of(command.Order.Id);
        var order = await dbContext.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == orderId, cancellationToken);

        if (order is null)
        {
            throw new OrderNotFoundException(command.Order.Id);
        }

        UpdateOrderWithNewValules(order, command.Order);
        dbContext.Orders.Update(order);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateOrderResult(true);
    }

    public void UpdateOrderWithNewValules(Order order, OrderDto orderDto)
    {
        var updatedShippingAddress = Address.Of
            (
                orderDto.ShippingAddress.FirstName,
                orderDto.ShippingAddress.LastName,
                orderDto.ShippingAddress.EmailAddress,
                orderDto.ShippingAddress.AddressLine,
                orderDto.ShippingAddress.Country,
                orderDto.ShippingAddress.State,
                orderDto.ShippingAddress.ZipCode
            );

        var updatedBillingAddres = Address.Of
         (
             orderDto.ShippingAddress.FirstName,
             orderDto.ShippingAddress.LastName,
             orderDto.ShippingAddress.EmailAddress,
             orderDto.ShippingAddress.AddressLine,
             orderDto.ShippingAddress.Country,
             orderDto.ShippingAddress.State,
             orderDto.ShippingAddress.ZipCode
         );

        var updatedPayment = Payment.Of
        (
            orderDto.Payment.CardName,
            orderDto.Payment.CardNumber,
            orderDto.Payment.Expiration,
            orderDto.Payment.Cvv,
            orderDto.Payment.PaymentMethod
        );

        order.Update
            (
                orderName: OrderName.Of(orderDto.OrderName),
                shippingAddress: updatedShippingAddress,
                billingAddress: updatedBillingAddres,
                payment: updatedPayment,
                status: orderDto.Status
            );

    }
}
