var builder = WebApplication.CreateBuilder(args);


//Add services to the container.

builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices();

//  .AddWebsServices();



var app = builder.Build();
//Configure the HTTP request pipeline.

//Ejecutar migraciones
app.UseApiServices();

if (app.Environment.IsDevelopment()){
    await app.InitialiseDatabaseAync();
}


app.Run();