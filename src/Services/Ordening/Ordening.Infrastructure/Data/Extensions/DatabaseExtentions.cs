using Microsoft.AspNetCore.Builder;

namespace Ordening.Infrastructure.Data.Extensions;
public static class DatabaseExtentions
{
    //Se utilizará en program.cs de Ordening
    //Este método de extensión se encarga de inicializar y migrar la base de datos automáticamente al iniciar la aplicación,
    //asegurando que la BD tenga el último esquema definido en tu proyecto
    public static async Task InitialiseDatabaseAync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Database.MigrateAsync().GetAwaiter().GetResult();

        await SeedAsync(context);
    }

    private static async Task SeedAsync(ApplicationDbContext context)
    {
        await SeedCustomerAsync(context);
        await SeedProductAsync(context);
        await SeedOrderAndItemsAsync(context);
    }

    public static async Task SeedCustomerAsync(ApplicationDbContext context)
    {
        if(!await context.Customers.AnyAsync())
        {
            await context.Customers.AddRangeAsync(InitialData.Customers);
            await context.SaveChangesAsync();
        }
    }

    public static async Task SeedProductAsync(ApplicationDbContext context)
    {
        if (!await context.Products.AnyAsync())
        {
            await context.Products.AddRangeAsync(InitialData.Products);
            await context.SaveChangesAsync();
        }
    }

    public static async Task SeedOrderAndItemsAsync(ApplicationDbContext context)
    {
        if (!await context.Orders.AnyAsync())
        {
            await context.Orders.AddRangeAsync(InitialData.OrderWithItems);
            await context.SaveChangesAsync();
        }
    }
}