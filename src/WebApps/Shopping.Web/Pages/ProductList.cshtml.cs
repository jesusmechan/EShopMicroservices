namespace Shopping.Web.Pages;

public class ProductListModel (ICatalogService catalogService, IBasketService basketService, ILogger<ProductListModel> logger) : PageModel
{
    public IEnumerable<string> CategoryList { get; set; } = [];
    public IEnumerable<ProductModel> ProductList { get; set; } = [];

    [BindProperty(SupportsGet = true)]
    public string SelectedCategory { get; set; } = default!;

    public async Task<IActionResult> OnGetAsync(string categoryName)
    {
        var response = await catalogService.GetProducts();
        CategoryList = response.Products.SelectMany(p => p.Category).Distinct();
        if (!string.IsNullOrWhiteSpace(categoryName))
        {
            ProductList = response.Products.Where(p => p.Category.Contains(categoryName)).ToList();
            SelectedCategory = categoryName;
        }
        else
        {
            ProductList = response.Products;
        }
        return Page();
    }

    public async Task<IActionResult> OnPostAddtoCartAsync(Guid productId)
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
