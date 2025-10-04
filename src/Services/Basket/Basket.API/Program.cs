var builder = WebApplication.CreateBuilder(args);
//Add services to the container.


//Application Services
var assembly = typeof(Program).Assembly;
builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

//Data Services
builder.Services.AddValidatorsFromAssembly(assembly); // Registra los validadores de FluentValidation
builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("CatalogConnection"));
    options.Schema.For<ShoppingCart>().Identity(x => x.UserName); // Configura la identidad del ShoppingCart por UserName
}).UseLightweightSessions();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
});


//Grpc Services
// TODO- Add Grpc Services
builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>(options =>
{
    options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]);
})
.ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler
    {
        ServerCertificateCustomValidationCallback =
        HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
    };
    return handler;
});

builder.Services.AddMessageBroker(builder.Configuration, assembly); //Configuración para rabbitmq

//Cross-Cutting Services
builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("CatalogConnection")!)
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!);



//Agrega los servicios de Swagger
builder.Services.AddEndpointsApiExplorer(); // Necesario para APIs m�nimas y Carter
builder.Services.AddSwaggerGen();          // Genera el documento Swagger



var app = builder.Build();


//Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    options.RoutePrefix = ""; // Muestra Swagger en la ra�z (http://localhost:5000/)
});


// Configure the HTTP request pipeline.
app.MapCarter();

app.UseExceptionHandler(options =>
{

});

app.UseHealthChecks("/health", 
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
    });

app.Run();
