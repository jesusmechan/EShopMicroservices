using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Shopping.Web.Models.Ordering;

namespace Shopping.Web.Pages
{
    public class OrdersListModel (IOrderingService orderingService, ILogger<OrdersListModel> logger) : PageModel
    {
        public IEnumerable<OrderModel> Orders { get; set; } = new  List<OrderModel>();
        public async Task<IActionResult> OnGetAsync()
        {
            //Assumption customerId is passes in form the UI authenticated user jesus.mechan
            var customerId = new Guid("58c49479-ec65-4de2-86e7-033c546291aa");

            var response = await orderingService.GetOrdersByCustomer(customerId);
            Orders = response.Orders;
            return Page();
        }
    }
}
