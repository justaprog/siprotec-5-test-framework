using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using TestManagement.Api.Data;

var builder = WebApplication.CreateBuilder(args);

// add services to the dependency injection container
// add controllers to the container to enable API endpoints
// add OpenAPI/Swagger generation to the container to enable API documentation and testing
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// to debug connection string
// make sure the connection string in appsettings.Development.json is correct and the database server is running
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//Console.WriteLine($"CONNECTION STRING = {connectionString}");

// register AppDbContext with the dependency injection container
// use PostgreSQL as the database provider and the connection string from configuration
// this allows us to inject AppDbContext into our controllers to access the database
// for example, in TestRunsController we can inject AppDbContext and use it to 
// query test runs and test cases
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
    });

}

app.UseHttpsRedirection();

app.UseAuthorization();
// map controller routes to enable API endpoints defined in controllers
app.MapControllers();

app.Run();