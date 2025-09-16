namespace Ordening.Application.Orders.Queries.GetOrderByName;
public class GetOrdersByNameHandler(IApplicationDbContext dbContext) : IQueryHandler<GetOrdersByNameQuery, GetOrdersByNameResult>
{
    public async Task<GetOrdersByNameResult> Handle(GetOrdersByNameQuery query, CancellationToken cancellationToken)
    {
        //get order by name using dbContext
        var order = await dbContext.Orders
            .Include(x => x.OrderItems)
            .AsNoTracking()
            .Where(x => x.OrderName.Value.Contains(query.Name))
            .OrderBy(x => x.OrderName.Value)
            .ToListAsync(cancellationToken);

        //var orderDtos = ProjectOrderDto(order);
        //ToOrderDtoList: Es un método de extensión de Order
        //return result
        return new GetOrdersByNameResult(order.ToOrderDtoList());
    }

    //public List<OrderDto> ProjectOrderDto(List<Order> orders)
    //{
    //    List<OrderDto> result = new();
    //    foreach (var order in orders)
    //    {
    //        var orderDto = new OrderDto
    //        (
    //            Id: order.Id.Value,
    //            CustomerId: order.CustomerId.Value,
    //            OrderName: order.OrderName.Value,
    //            ShippingAddress: new AddressDto
    //                (
    //                    FirstName: order.ShippingAddress.FirstName,
    //                    LastName: order.ShippingAddress.LastName,
    //                    EmailAddress: order.ShippingAddress.EmailAddress?? "",
    //                    AddressLine: order.ShippingAddress.AddressLine,
    //                    Country: order.ShippingAddress.Country,
    //                    State: order.ShippingAddress.State,
    //                    ZipCode: order.ShippingAddress.ZipCode
    //                ),
    //            BillingAddress: new AddressDto
    //                (
    //                    FirstName: order.BillingAddress.FirstName,
    //                    LastName: order.BillingAddress.LastName,
    //                    EmailAddress: order.BillingAddress.EmailAddress??"",
    //                    AddressLine: order.BillingAddress.AddressLine,
    //                    Country: order.BillingAddress.Country,
    //                    State: order.BillingAddress.State,
    //                    ZipCode: order.BillingAddress.ZipCode
    //                ),
    //            Payment: new PaymentDto
    //                (
    //                    CardName: order.Payment.CardName?? "",
    //                    CardNumber: order.Payment.CardNumber,
    //                    Expiration: order.Payment.Expiration,
    //                    Cvv: order.Payment.CVV,
    //                    PaymentMethod: order.Payment.PaymentMethod
    //                ),
    //            Status: order.Status,
    //            OrderItems: order.OrderItems
    //                            .Select(x => new OrderItemDto
    //                            (
    //                                x.OrderId.Value,
    //                                x.ProductId.Value,
    //                                x.Quantity,
    //                                x.Price
    //                            )).ToList()

    //        );
    //    }
    //    return result;
    //}
}
