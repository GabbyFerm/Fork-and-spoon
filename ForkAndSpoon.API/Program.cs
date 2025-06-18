using ForkAndSpoon.API.Middleware;
using ForkAndSpoon.Application;
using ForkAndSpoon.Infrastructure;
using ForkAndSpoon.Infrastructure.Database;
using ForkAndSpoon.Infrastructure.Database.Seeders;
using ForkAndSpoon.API.Helpers;
using ForkAndSpoon.Infrastructure.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Application & Infrastructure
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

// Controllers & API Docs
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerDocumentation();

// JWT Generator & Auth
builder.Services.AddScoped<JWTGenerator>();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddAuthorization();

// CORS (for frontend connection)
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Build and run
var app = builder.Build();

// Seeder
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ForkAndSpoonDbContext>();
    var seeder = new DbSeeder(dbContext);
    await seeder.SeedAsync();
}

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors("FrontendPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
await app.RunAsync();