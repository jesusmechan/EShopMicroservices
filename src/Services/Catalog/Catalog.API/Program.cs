var builder = WebApplication.CreateBuilder(args);

//Add services to the container.
var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>)); // Agrega el comportamiento de validación
    config.AddOpenBehavior(typeof(LoggingBehavior<,>)); // Agrega el comportamiento de validación
});
builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddCarter();

builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("CatalogConnection"));
}).UseDirtyTrackedSessions(); // Usa sesiones con seguimiento de cambios


if(builder.Environment.IsDevelopment())
{
    // Solo para desarrollo, habilita el logging de Marten
    builder.Services.InitializeMartenWith<CatalogInitialData>();
}


builder.Services.AddExceptionHandler<CustomExceptionHandler>(); // Agrega el manejador de excepciones personalizado


//Agrega los servicios de Swagger
builder.Services.AddEndpointsApiExplorer(); // Necesario para APIs mínimas y Carter
builder.Services.AddSwaggerGen();          // Genera el documento Swagger

builder.Services.AddHealthChecks() // Agrega servicios de health checks
       .AddNpgSql(
           builder.Configuration.GetConnectionString("CatalogConnection")); // Agrega un health check para la base de datos

var app = builder.Build();

//Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    options.RoutePrefix = ""; // Muestra Swagger en la raíz (http://localhost:5000/)
});
app.MapCarter();

app.UseExceptionHandler(options => {});


app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    }); // Mapea el endpoint de health checks

app.Run();
