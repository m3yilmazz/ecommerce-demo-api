using Application.API.Middlewares;
using Application.API.Validations.Customers;
using Application.Application;
using AspNetCoreRateLimit;
using Core.Data;
using FluentValidation;
using FluentValidation.AspNetCore;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterIpRateLimiting(builder.Configuration);

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.Console(new Serilog.Formatting.Json.JsonFormatter())
    .WriteTo.File("logs/api-log-.txt", rollingInterval: RollingInterval.Day)
    .Enrich.FromLogContext()
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllers();
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
builder.Services.RegisterLoggerPipelineBehavior();
builder.Services.RegisterAuditLogPipelineBehavior();

var app = builder.Build();

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

app.UseSerilogRequestLogging();

app.UseCors(s => s.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseExceptionHandler(s => s.Run(ExceptionHandler.Handle));

app.UseAuthorization();

app.MapControllers();

app.Run();