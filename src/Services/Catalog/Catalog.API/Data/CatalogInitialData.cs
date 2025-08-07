using Marten.Schema;
using Microsoft.AspNetCore.Identity;

namespace Catalog.API.Data;
public class CatalogInitialData : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        using var session = store.LightweightSession();
        if (await session.Query<Product>().AnyAsync()) 
            return;

        session.Store<Product>(GetPreconfiguredProducts());
        await session.SaveChangesAsync(cancellation);

    }

    public static IEnumerable<Product> GetPreconfiguredProducts() => new List<Product>
    {
        new Product()
        {
            Id = new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d479"),
            Name = "Iphone X",
            Description = "Iphone X is a smartphone that was tested with the iOS 11.0 operating system. This model weighs 6.14 ounces, has a 5.8 inch display, 12-megapixel main camera, and 7-megapixel selfie camera.",
            ImageFile = "https://images.unsplash.com/photo-1506748686214-e9df14d4d9d0?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=800&q=80",
            Price = 999.99M,
            Category = new List<string> { "Smartphones", "Electronics" }
        },
        new Product
        {
            Id = new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d480"),
            Name = "Samsung Galaxy S10",
            Description = "Samsung Galaxy S10 is a smartphone that was tested with the Android 9.0 operating system. This model weighs 5.51 ounces, has a 6.1 inch display, 12-megapixel main camera, and 10-megapixel selfie camera.",
            ImageFile = "https://images.unsplash.com/photo-1556740749-887f6717d7e4?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=800&q=80",
            Price = 899.99M,
            Category = new List<string> { "Smartphones", "Electronics" }
        },
        new Product
        {
            Id = new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d481"),
            Name = "Google Pixel 3",
            Description = "Google Pixel 3 is a smartphone that was tested with the Android 9.0 operating system. This model weighs 5.08 ounces, has a 5.5 inch display, 12-megapixel main camera, and 8-megapixel selfie camera.",
            ImageFile = "https://images.unsplash.com/photo-1556740749-887f6717d7e4?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=800&q=80",
            Price = 799.99M,
            Category = new List<string> { "Smartphones", "Electronics" }
        },
        new Product
        {
            Id = new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d482"),
            Name = "OnePlus 6T",
            Description = "OnePlus 6T is a smartphone that was tested with the Android 9.0 operating system. This model weighs 5.41 ounces, has a 6.41 inch display, 16-megapixel main camera, and 16-megapixel selfie camera.",
            ImageFile = "https://images.unsplash.com/photo-1556740749-887f6717d7e4?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=800&q=80",
            Price = 549.99M,
            Category = new List<string> { "Smartphones", "Electronics" }
        },
        new Product
        {
            Id = new Guid("f47ac10b-58cc-4372-a567-0e02b2c3d483"),
            Name = "Huawei P30 Pro",
            Description = "Huawei P30 Pro is a smartphone that was tested with the Android 9.0 operating system. This model weighs 6.39 ounces, has a 6.47 inch display, 40-megapixel main camera, and 32-megapixel selfie camera.",
            ImageFile = "https://images.unsplash.com/photo-1556740749-887f6717d7e4?ixlib=rb-1.2.1&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=800&q=80",
            Price = 999.99M,
            Category = new List<string> { "Smartphones", "Electronics" }
        }

    };
} 

