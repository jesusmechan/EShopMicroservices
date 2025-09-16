namespace Ordening.Application.Dtos.Order;
public record OrderItemDto
(
    Guid OrderId,
    Guid ProdcutId,
    int Quantity,
    decimal Price
);