using Shopping.Web.Models.Ordering;

namespace Shopping.Web.Services
{
    public interface IOrderingService
    {
        [Get("/ordering-service/orders?pageIndex={pageIndex}&pageSize={pageSize}\"")]
        Task<GetOrdersResponse> GetOrders(int? PageIndex = 1, int? PageSize = 10);

        [Get("/ordering-service/orders/byname/{orderName}")]
        Task<GetOrdersByNameResponse> GetOrdersByName(string orderName);

        [Get("/ordering-service/orders/bycustomer/{customerId}")]
        Task<GetOrdersByCustomerResponse> GetOrdersByCustomer(Guid customerId);
    }
}
 