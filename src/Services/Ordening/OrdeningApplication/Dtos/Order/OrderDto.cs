namespace Ordening.Application.Dtos.Order;

public record OrderDto
(
    Guid Id,
    Guid CustomerId,
    string OrderName,
    AddressDto ShippingAddress, //Dirección de envío.
    AddressDto BillingAddress, //Dirección de facturación.
    PaymentDto Payment,
    OrderStatus Status,
    List<OrderItemDto> OrderItems

);