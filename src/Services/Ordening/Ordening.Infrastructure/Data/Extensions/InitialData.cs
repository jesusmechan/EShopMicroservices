namespace Ordening.Infrastructure.Data.Extensions;

public class InitialData
{
    public static IEnumerable<Customer> Customers =>
        new List<Customer>
        {
            Customer.Create(CustomerId.Of(new Guid("58c49479-ec65-4de2-86e7-033c546291aa")),"Jesús Mechan Gonzales", "jesusmanuelmechan@gmail.com"),
            Customer.Create(CustomerId.Of(new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61")),"Miguel Alarcón", "miguel@gmail.com")
        };

    public static IEnumerable<Product> Products =>
    new List<Product>
    {
            Product.Create(ProductId.Of(new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c3301")),"Iphone X", 500),
            Product.Create(ProductId.Of(new Guid("9c9d3b2e-2c47-4f55-8e56-12f5b7b9e78c")),"Samsung 10", 400),
            Product.Create(ProductId.Of(new Guid("1d8a6b37-0f44-4af6-bba6-8c7e3c4d5f6a")),"Huawei Plus", 650),
            Product.Create(ProductId.Of(new Guid("7b9e3c18-9a42-4c3b-83f4-6d2f9f7c1d5e")),"Xiaomi Mi", 450)
    };

    public static IEnumerable<Order> OrderWithItems
    {
        get
        {
            var address1 = Address.Of("mehmt", "ozkaya", "mehmet@gmail.com", "Bahcelivler No:4", "Turkey", "Istanbul", "38052");
            var address2 = Address.Of("jhon", "doe", "jhon@gmail.com", "Broadway No:1", "England", "Nottingham", "08050");

            var payment1 = Payment.Of("mehmet", "55555555555554444", "12/28", "355", 1);
            var payment2 = Payment.Of("mehmet", "88855555555554444", "06/30", "222", 2);

            var order1 = Order.Create
            (
                OrderId.Of(Guid.NewGuid()),
                CustomerId.Of(new Guid("58c49479-ec65-4de2-86e7-033c546291aa")),
                OrderName.Of("ORD_1"),
                shippingAddress: address1,
                billingAddress: address1,
                payment: payment1
            );

            order1.Add(ProductId.Of(new Guid("3f2504e0-4f89-11d3-9a0c-0305e82c3301")), 2, 500);
            order1.Add(ProductId.Of(new Guid("9c9d3b2e-2c47-4f55-8e56-12f5b7b9e78c")), 1, 400);

            var order2 = Order.Create
            (
                OrderId.Of(Guid.NewGuid()),
                CustomerId.Of(new Guid("5334c996-8457-4cf0-815c-ed2b77c4ff61")),
                OrderName.Of("ORD_2"),
                shippingAddress: address2,
                billingAddress: address2,
                payment: payment2
            );

            order2.Add(ProductId.Of(new Guid("1d8a6b37-0f44-4af6-bba6-8c7e3c4d5f6a")), 2, 450);
            order2.Add(ProductId.Of(new Guid("7b9e3c18-9a42-4c3b-83f4-6d2f9f7c1d5e")), 1, 650);

            return new List<Order> { order1, order2 };
        }
    }

}
