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

// Configure the DbContext with SQL Server
services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Swagger for API documentation
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "My API", Version = "v1" });
});

// Add CORS policy to allow requests from localhost:3000
services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // Allow specific frontend origin
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials(); // Allow credentials if needed
    });
});

var app = builder.Build();

// Enable Swagger in both development and production
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseHsts(); // Enforce strict transport security in production
}
else
{
    app.UseHttpsRedirection(); // Allow HTTPS redirection in development
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
