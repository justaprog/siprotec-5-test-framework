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

// CORS: allow the Vite frontend during development
var AllowFrontend = "_allowFrontend";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowFrontend, policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
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
// enable the CORS middleware, should be placed before UseAuthorization and 
// MapControllers to ensure CORS headers are included in responses
// to preflight requests and API endpoints
app.UseCors(AllowFrontend);

app.UseAuthorization();
// map controller routes to enable API endpoints defined in controllers
app.MapControllers();

app.Run();