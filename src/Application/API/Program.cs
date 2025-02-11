using Application.API.Middlewares;
using Application.API.Validations.Customers;
using Application.Application;
using AspNetCoreRateLimit;
using Core.Data;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterIpRateLimiting(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});

builder.Services
    .AddFluentValidationAutoValidation()
    .AddValidatorsFromAssemblyContaining<CreateCustomerRequestValidator>();

builder.Services.AddPostgres(builder.Configuration);
builder.Services.AddRepositories();

builder.Services.RegisterMediatR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<Context>();

    dbContext.Database.Migrate();
}

app.UseIpRateLimiting();

app.UseCors(s => s.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseExceptionHandler(s => s.Run(ExceptionHandler.Handle));

app.UseAuthorization();

app.MapControllers();

app.Run();
