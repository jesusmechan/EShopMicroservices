
namespace Ordening.Application.Orders.Commands.DeleteOrder;

public class DeleteOrderHandler(IApplicationDbContext dbContext) : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
{
    public async Task<DeleteOrderResult> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
    {
        //Delete Order entity from command object
        var orderId = OrderId.Of(command.OrderId);
        var order = await dbContext.Orders.FindAsync(orderId, cancellationToken);

        if(order is null)
        {
            throw new OrderNotFoundException(command.OrderId);
        }
        //Save to database
        dbContext.Orders.Remove(order);
        await dbContext.SaveChangesAsync(cancellationToken);

        //return result
        return new DeleteOrderResult(true);
    }
}
