var builder = WebApplication.CreateBuilder(args);

//Add services to the container.
builder.Services.AddCarter();


var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});

builder.Services.AddValidatorsFromAssembly(assembly); // Registra los validadores de FluentValidation

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("CatalogConnection"));
    options.Schema.For<ShoppingCart>().Identity(x => x.UserName); // Configura la identidad del ShoppingCart por UserName
}).UseLightweightSessions();


//Agrega los servicios de Swagger
builder.Services.AddEndpointsApiExplorer(); // Necesario para APIs m�nimas y Carter
builder.Services.AddSwaggerGen();          // Genera el documento Swagger

builder.Services.AddScoped<IBasketRepository, BasketRepository>();

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

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

app.Run();
