using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;
using TestManagement.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// to debug connection string
// make sure the connection string in appsettings.Development.json is correct and the database server is running
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//Console.WriteLine($"CONNECTION STRING = {connectionString}");

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

app.MapControllers();

app.Run();