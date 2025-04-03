using ForkAndSpoon.Application;
using ForkAndSpoon.Infrastructure;
using ForkAndSpoon.Infrastructure.Helpers;
using ForkAndSpoon.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Application & Infrastructure
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Controllers 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add Swagger/OpenAPI
builder.Services.AddSwaggerDocumentation();

// JWT Generator
builder.Services.AddScoped<JWTGenerator>();

// Authentication
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

// Build and run
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();