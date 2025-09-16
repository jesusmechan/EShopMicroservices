
using BuildingBlocks.Pagination;

namespace Ordening.Application.Orders.Queries.GetOrders;

public class GetOrdersHandler (IApplicationDbContext dbContext) : IQueryHandler<GetOrdersQuery, GetOrdersResult>
{
    public async Task<GetOrdersResult> Handle(GetOrdersQuery query, CancellationToken cancellationToken)
    {
        //get orders with pagination
        var pageIndex = query.PaginationRequest.PageIndex;
        var pageSize = query.PaginationRequest.PageSize;
        var totalCount = await dbContext.Orders.LongCountAsync(); //obtener el total de registros.
        var orders = await dbContext.Orders
                        .Include(x => x.OrderItems)
                        .OrderBy(x => x.OrderName.Value)
                        .Skip((pageIndex - 1) * pageSize) //ignora los registros que indique //ejmplo pageIndex 2 y pageSize 10 = 2 * 10 = 20, ignorará los primeros 20 registros.
                        .Take(pageSize) //tomará la cantidad de registros.
                        .ToListAsync(cancellationToken);
        return new GetOrdersResult
        (
          new PaginationResult<OrderDto>
          (
              pageIndex,
              pageSize,
              totalCount,
              orders.ToOrderDtoList()
          )
        );
    }
}
