using ForkAndSpoon.Application;
using ForkAndSpoon.Infrastructure;
using ForkAndSpoon.Infrastructure.Database;
using ForkAndSpoon.Infrastructure.Database.Seeders;
using ForkAndSpoon.Infrastructure.Extensions;
using ForkAndSpoon.Infrastructure.Helpers;

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

// Seeder
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ForkAndSpoonDbContext>();
    var seeder = new DbSeeder(dbContext);
    await seeder.SeedAsync();
}


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
await app.RunAsync();