using Microsoft.EntityFrameworkCore;
using StoreProject.Data;
using StoreProject.Interfaces;
using StoreProject.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
var services = builder.Services;

// Add scoped service for the repository
services.AddScoped<IStoreRepository, StoreRepository>();

// Add controllers with views
services.AddControllersWithViews();

// Determine the environment
var environment = builder.Environment;

// Configure the DbContext with SQL Server
services.AddDbContext<DataContext>(options =>
{
    var connectionString = environment.IsDevelopment()
        ? builder.Configuration.GetConnectionString("DevelopmentConnection") // Development database
        : builder.Configuration.GetConnectionString("ProductionConnection"); // Production database

    options.UseSqlServer(connectionString);
});

// Add Swagger for API documentation (enabled only in Development)
if (environment.IsDevelopment())
{
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new() { Title = "My API", Version = "v1" });
    });
}

// Add CORS policy
services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(environment.IsDevelopment()
                ? "http://localhost:3000" // Development frontend
                : "https://your-production-frontend.com") // Production frontend
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Allow credentials if needed
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (environment.IsDevelopment())
{
    // Enable Swagger in development
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });

    app.UseHttpsRedirection(); // Allow HTTPS redirection in development
}
else
{
    app.UseHsts(); // Enforce strict transport security in production
}

// Enable static files
app.UseStaticFiles();

// Enable CORS policy
app.UseCors("AllowFrontend");

// Enable routing
app.UseRouting();

// Authorization middleware
app.UseAuthorization();

// Map default controller routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

// Serve the fallback file for SPA apps (if applicable)
app.MapFallbackToFile("index.html");

// Run the application
app.Run();
