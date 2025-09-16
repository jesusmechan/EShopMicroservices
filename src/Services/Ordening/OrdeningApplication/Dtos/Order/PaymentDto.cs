namespace Ordening.Application.Dtos.Order;

public record PaymentDto
(
    string CardName,
    string CardNumber,
    string Expiration,
    string Cvv,
    int PaymentMethod
);