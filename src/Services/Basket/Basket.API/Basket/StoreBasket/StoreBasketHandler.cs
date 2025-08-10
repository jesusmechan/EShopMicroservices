namespace Basket.API.Basket.StoreBasket;

public record StoreBasketCommand(ShoppingCart Cart) : ICommand<StoreBasketResult>;
public record StoreBasketResult(string UserName);

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Cart)
            .NotNull()
            .WithMessage("Shopping cart cannot be null.")
            .Must(cart => cart.Items.Any())
            .WithMessage("Shopping cart must contain at least one item.");
        
        RuleFor(x => x.Cart.UserName)
            .NotEmpty()
            .WithMessage("UserName is required.");
    }
}

public class StoreBasketCommandHandler 
    (IBasketRepository repository)
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        ShoppingCart cart = command.Cart;
        //TODO: Store basket in database (Use marten upsert - if exists - update, if not exist).
        //TODO: update cache
        await repository.StoreBasket(command.Cart, cancellationToken);
        return new StoreBasketResult(command.Cart.UserName);
    }
}

