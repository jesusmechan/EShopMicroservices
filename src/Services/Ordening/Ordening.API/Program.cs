using Ordening.Application;
using Ordening.Infrastructure;
using Ordening.API;

var builder = WebApplication.CreateBuilder(args);


//Add services to the container.

builder.Services
    .AddApplicationServices()
    .AddInfrastructureServices(builder.Configuration)
    .AddApiServices();

//  .AddWebsServices();



var app = builder.Build();


//Configure the HTTP request pipeline.
app.Run();