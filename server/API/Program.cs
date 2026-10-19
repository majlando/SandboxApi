using LinqToDB;
using LinqToDB.AspNet;
using Infrastructure.Data;
using Service;

var builder = WebApplication.CreateBuilder(args);

// 1. Register Controllers, OpenAPI, and Exception Handling
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddProblemDetails();
builder.Services.AddAuthorization();

// 2. Register linq2db Context
var connectionString = builder.Configuration.GetConnectionString("Default")
                       ?? throw new InvalidOperationException("Connection string 'Default' is not configured.");

builder.Services.AddLinqToDBContext<AppDataConnection>((provider, options) =>
    options.UseSQLite(connectionString));

// 3. Register Service Layer in DI Container
builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();
app.MapControllers();

app.Run();