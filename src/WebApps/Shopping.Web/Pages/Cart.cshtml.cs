namespace Shopping.Web.Pages;
public class CartModel (IBasketService basketService, ILogger<CartModel> logger) : PageModel
{

    public ShoppingCartModel Cart { get; set; } = new ShoppingCartModel();

    public async Task<IActionResult> OnGetAsync()
    {
        Cart = await basketService.LoadUserBasket();
        return Page();
    }

    public async Task<IActionResult> OnPostRemoveToCartAsync(Guid productId)
    {
        logger.LogInformation("Remove to cart button clicked");
        
        //Obtenemos la cesta actual.
        Cart = await basketService.LoadUserBasket();

        //Removemos el producto de la lista.
        Cart.Items.RemoveAll(x => x.ProductId.Equals(productId));

        //Enviamos nuevamente la lista sin el producto eliminado.
        await basketService.StoreBasket(new StoreBasketRequest(Cart));

        return RedirectToPage();
    }
}
