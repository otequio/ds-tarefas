using Infrastructure;
using Application.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using Api.Middleware;
using Api.DependencyInjection;
using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices();
builder.Services.AddApplication();
builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("Infrastructure")
    )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference(@"api");
}
app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseRateLimiter();
app.MapControllers()
   .RequireRateLimiting("fixed");
app.UseCors();
app.Run();
