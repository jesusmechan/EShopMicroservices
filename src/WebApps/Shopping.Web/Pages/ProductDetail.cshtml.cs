using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Shopping.Web.Pages
{
    public class ProductDetailModel (ICatalogService catalogService, IBasketService basketService, ILogger<ProductDetailModel> logger) : PageModel
    {
        public ProductModel Product { get; set; } = default!;


        [BindProperty]
        public string Color { get; set; } = default!;


        [BindProperty]
        public int Quantity { get; set; } = default!;


        public async Task<IActionResult> OnGetAsync(Guid productId)
        {
            logger.LogInformation("Ejecutando OnGetProductDetailAsync");
            var response = await catalogService.GetProduct(productId);
            Product = response.Product;
            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(Guid productId)
        {
            logger.LogInformation("Add to cart button checked");
            var product = await catalogService.GetProduct(productId);

            var basket = await basketService.LoadUserBasket();

            basket.Items.Add(new ShoppingCartItemModel
            {
                ProductId = productId,
                ProductName = product.Product.Name,
                Price = product.Product.Price,
                Quantity = 1,
                Color = "Black"
            });

            await basketService.StoreBasket(new StoreBasketRequest(basket));
            return RedirectToPage("Cart");
        }
    }
}
