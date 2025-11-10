namespace Security.Infrastrucure.Data.Extensions;
public static class DatabaseExtentions
{
    //Se utilizará en program.cs de Security
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
        //Aquí puedes agregar métodos para sembrar datos iniciales si es necesario
        await SeedDocumentType(context);
        await SeedPersons(context);
        await SeedRoles(context);
    }

    public static async Task SeedDocumentType(ApplicationDbContext context)
    {
        if(!await context.DocumentTypes.AnyAsync())
        {
            await context.DocumentTypes.AddRangeAsync(InitialData.DocumentTypes);
            await context.SaveChangesAsync();
        }
    }

    public static async Task SeedPersons(ApplicationDbContext context)
    {
        if(!await context.Persons.AnyAsync())
        {
            await context.Persons.AddRangeAsync(InitialData.Persons);
            await context.SaveChangesAsync();
        }
    }

    public static async Task SeedRoles(ApplicationDbContext context)
    {
        if (!await context.Roles.AnyAsync())
        {
            await context.Roles.AddRangeAsync(InitialData.Roles);
            await context.SaveChangesAsync();
        }
    }
}
