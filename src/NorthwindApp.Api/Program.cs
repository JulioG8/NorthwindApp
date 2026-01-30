using Microsoft.EntityFrameworkCore;
using NorthwindApp.Application.Interfaces;
using NorthwindApp.Application.Services;
using NorthwindApp.Domain.Interfaces;
using NorthwindApp.Infrastructure.Data;
using NorthwindApp.Infrastructure.Repositories;
using NorthwindApp.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Entity Framework Core
builder.Services.AddDbContext<NorthwindDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("NorthwindConnection")));

// Register repositories (Dependency Injection)
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IWebTrackerRepository, WebTrackerRepository>();

// Register services (Dependency Injection)
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline - Swagger always enabled
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("AllowAll");

// Serve static files (frontend)
app.UseDefaultFiles();
app.UseStaticFiles();

// WebTracker Middleware - logs all requests
app.UseMiddleware<WebTrackerMiddleware>();

app.UseAuthorization();
app.MapControllers();

app.Run();
