using Ambev.DeveloperEvaluation.Application;
using Ambev.DeveloperEvaluation.Application.Interfaces;
using Ambev.DeveloperEvaluation.Common.HealthChecks;
using Ambev.DeveloperEvaluation.Common.Logging;
using Ambev.DeveloperEvaluation.Common.Security;
using Ambev.DeveloperEvaluation.Common.Validation;
using Ambev.DeveloperEvaluation.IoC;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Logging;
using Ambev.DeveloperEvaluation.ORM.MongoDb;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Ambev.DeveloperEvaluation.WebApi.Middleware;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Serilog;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.WebApi;

/// <summary>
/// Entry point for the Ambev Developer Evaluation Web API application.
/// </summary>
public class Program
{
    /// <summary>
    /// Main entry point for the application.
    /// </summary>
    /// <param name="args">Command-line arguments</param>
    public static void Main(string[] args)
    {
        try
        {
            Log.Information("Starting Ambev Developer Evaluation Web API");

            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            // Configure logging first
            builder.AddDefaultLogging();

            //  Basic services
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configure database - with error handling
            try
            {
                var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("DefaultConnection string not found in configuration");
                }

                builder.Services.AddDbContext<DefaultContext>(options =>
                    options.UseNpgsql(
                        connectionString,
                        b => b.MigrationsAssembly("Ambev.DeveloperEvaluation.ORM")
                    )
                );
                Log.Information("Database context configured successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to configure database context");
                throw;
            }

            //  Configure MongoDB - with error handling
            try
            {
                builder.Services.AddSingleton<IMongoDbContext, MongoDbContext>();
                Log.Information("MongoDB context configured successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to configure MongoDB context");
                throw;
            }

            //  Register core dependencies FIRST
            try
            {
                builder.RegisterDependencies();
                Log.Information("Dependencies registered successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to register dependencies");
                throw;
            }

            //Configure specific services
            builder.Services.AddScoped<ISaleAuditRepository, MongoSaleAuditRepository>();
            builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

            //  Configure AutoMapper - with specific assemblies
            try
            {
                builder.Services.AddAutoMapper(cfg =>
                {
                    cfg.AllowNullCollections = true;
                    cfg.AllowNullDestinationValues = true;
                }, typeof(ApplicationLayer).Assembly);

                Log.Information("AutoMapper configured successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to configure AutoMapper");
                throw;
            }

           

            //  Configure MediatR
            try
            {
                builder.Services.AddMediatR(cfg =>
                {
                    cfg.RegisterServicesFromAssembly(typeof(ApplicationLayer).Assembly);
                });
                Log.Information("MediatR configured successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to configure MediatR");
                throw;
            }

            //  Configure validation pipeline
            builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            //  Configure authentication
            try
            {
                builder.Services.AddJwtAuthentication(builder.Configuration);
                Log.Information("JWT Authentication configured successfully");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to configure JWT Authentication");
                throw;
            }

            //  Configure health checks
            builder.AddBasicHealthChecks();

            Log.Information("Building application...");
            var app = builder.Build();

            // Test service resolution
            try
            {
                using (var scope = app.Services.CreateScope())
                {
                    // Test critical services
                    var dbContext = scope.ServiceProvider.GetRequiredService<DefaultContext>();
                    var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                    Log.Information("Critical services resolved successfully");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed to resolve critical services");
                throw;
            }

            //  Configure middleware pipeline
            app.UseMiddleware<ValidationExceptionMiddleware>();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseBasicHealthChecks();
            app.MapControllers();

            Log.Information("Application configured successfully");
            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");

            //  Detailed error logging
            if (ex is AggregateException aggEx)
            {
                Log.Fatal("AggregateException details:");
                foreach (var innerEx in aggEx.InnerExceptions)
                {
                    Log.Fatal(innerEx, "Inner exception: {Message}", innerEx.Message);
                }
            }

            // Additional error details
            Console.WriteLine($"\n=== DETAILED ERROR ===");
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine($"Type: {ex.GetType().Name}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");

            if (ex.InnerException != null)
            {
                Console.WriteLine($"\nInner Exception: {ex.InnerException.Message}");
                Console.WriteLine($"Inner Type: {ex.InnerException.GetType().Name}");
            }

            throw;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}