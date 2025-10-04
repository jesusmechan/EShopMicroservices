using BuildingBlocks.Messaging.Events;
using MassTransit;
using Ordening.Application.Orders.Commands.CreateOrder;


//EVENTO DE INTEGRACIÓN - RECIBE O EVENTO DO BASKET
namespace Ordening.Application.Orders.EventHandlers.Integration;
public class BasketCheckoutEventHandler (ISender sender, ILogger<BasketCheckoutEventHandler> logger) : IConsumer<BasketCheckoutEvent>
{
    public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
    {
        //TODO: Create new order and start order fulltillment process

        logger.LogInformation("Integration Event handled: {IntegrationEvent}", context.Message.GetType().Name);


        var command = MapToCreateOrderCommand(context.Message);
        await sender.Send(command);

    }


    private CreateOrderCommand MapToCreateOrderCommand(BasketCheckoutEvent message)
    {
        //Create full order with incoming event data
        var addressDto = new AddressDto
        (
            message.FirstName,
            message.LastName,
            message.EmailAddress,
            message.AddressLine,
            message.Country,
            message.State,
            message.ZipCode
        );

        var paymentDto = new PaymentDto
        (
            message.CardName,
            message.CardNumber,
            message.Expiration,
            message.CVV,
            message.PaymentMethod
        );

        var orderId = Guid.NewGuid();

        var orderDto = new OrderDto
        (
            Id: orderId,
            CustomerId: message.CustomerId,
            OrderName: message.UserName,
            ShippingAddress: addressDto,
            BillingAddress: addressDto,
            Payment: paymentDto,
            Status: Ordening.Domain.Enums.OrderStatus.Pending,
            OrderItems:
            [
                new OrderItemDto
                (
                    orderId,
                    new Guid("3F2504E0-4F89-11D3-9A0C-0305E82C3301"),
                    2,
                    500
                ),
                    new OrderItemDto
                (
                    orderId,
                    new Guid("9C9D3B2E-2C47-4F55-8E56-12F5B7B9E78C"),
                    1,
                    400
                )
            ]

        );
        return new CreateOrderCommand(orderDto);
    }

}