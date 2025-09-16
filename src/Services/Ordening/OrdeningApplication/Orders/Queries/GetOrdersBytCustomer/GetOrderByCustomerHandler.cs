
namespace Ordening.Application.Orders.Queries.GetOrdersBytCustomer;
public class GetOrderByCustomerHandler(IApplicationDbContext dbContext) : IQueryHandler<GetOrdersByCustomerQuery, GetOrdersByCustomerResult>
{
    public async Task<GetOrdersByCustomerResult> Handle(GetOrdersByCustomerQuery query, CancellationToken cancellationToken)
    {
        //get orders by customer using dbcontext
        var order = await dbContext.Orders
                    .Include(x => x.OrderItems)
                    .AsNoTracking()
                    .Where(x => x.CustomerId == CustomerId.Of(query.CustomerId))
                    .OrderBy(x => x.OrderName.Value)
                    .ToListAsync(cancellationToken);

        //return result
        return new GetOrdersByCustomerResult(order.ToOrderDtoList());
    }
}
