using Carter;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container.


builder.Services.AddCarter();
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});


builder.Services.AddMarten(options =>
{
    options.Connection(builder.Configuration.GetConnectionString("CatalogConnection"));
}).UseDirtyTrackedSessions(); // Usa sesiones con seguimiento de cambios


//Agrega los servicios de Swagger
builder.Services.AddEndpointsApiExplorer(); // Necesario para APIs mínimas y Carter
builder.Services.AddSwaggerGen();          // Genera el documento Swagger

var app = builder.Build();

//Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
    options.RoutePrefix = ""; // Muestra Swagger en la raíz (http://localhost:5000/)
});
app.MapCarter();

app.Run();
