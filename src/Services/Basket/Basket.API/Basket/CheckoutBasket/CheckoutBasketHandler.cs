namespace Basket.API.Basket.CheckoutBasket;
public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckoutDto) : ICommand<CheckoutBasketResult>;
public record CheckoutBasketResult(bool Success);


public class CheckoutBasketCommandValidator : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.BasketCheckoutDto).NotNull().WithMessage("BasketCheckoutDto can't ");
        RuleFor(x => x.BasketCheckoutDto.UserName).NotEmpty().WithMessage("UserName is required.");
        //RuleFor(x => x.BasketCheckoutDto.CustomerId).NotEmpty();
        //RuleFor(x => x.BasketCheckoutDto.TotalPrice).GreaterThan(0);
        ////Shipping and BillingAddress
        //RuleFor(x => x.BasketCheckoutDto.FirstName).NotEmpty();
        //RuleFor(x => x.BasketCheckoutDto.LastName).NotEmpty();
        //RuleFor(x => x.BasketCheckoutDto.EmailAddress).NotEmpty().EmailAddress();
        //RuleFor(x => x.BasketCheckoutDto.AddressLine).NotEmpty();
        //RuleFor(x => x.BasketCheckoutDto.Country).NotEmpty();
        //RuleFor(x => x.BasketCheckoutDto.State).NotEmpty();
        //RuleFor(x => x.BasketCheckoutDto.ZipCode).NotEmpty();
        ////Payment
        //RuleFor(x => x.BasketCheckoutDto.CardName).NotEmpty();
        //RuleFor(x => x.BasketCheckoutDto.CardNumber).NotEmpty().CreditCard();
        //RuleFor(x => x.BasketCheckoutDto.Expiration).NotEmpty();
        //RuleFor(x => x.BasketCheckoutDto.CVV).NotEmpty().Length(3, 4);
        //RuleFor(x => x.BasketCheckoutDto.PaymentMethod).InclusiveBetween(1, 3);
    }
}

public class CheckoutBasketHandler
    (IBasketRepository respository, IPublishEndpoint publishEndpoint)
    : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    {
        //get existing bastket with total price
        var basket = await respository.GetBasket(command.BasketCheckoutDto.UserName, cancellationToken);
        if (basket is null)
            return new CheckoutBasketResult(false);

        //Set totalprice on basketcheckout event message
        //Send basketcheckout event to rabbitmq using masstransit
        var eventMesssage = command.BasketCheckoutDto.Adapt<BasketCheckoutEvent>();
        eventMesssage.TotalPrice = basket.TotalPrice;
        eventMesssage.Items = basket.Items.Adapt<List<ProductsInBasket>>();
        await publishEndpoint.Publish(eventMesssage, cancellationToken);

        //delete the basket
        await respository.DeleteBasket(command.BasketCheckoutDto.UserName, cancellationToken);

        return new CheckoutBasketResult(true);
    }
}