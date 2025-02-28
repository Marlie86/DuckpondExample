using DuckpondExample.Gateway.ApiClients;
using DuckpondExample.Gateway.Models;
using DuckpondExample.Gateway.Services;
using DuckpondExample.Shared.Common.Hosts.Extensions;

using Microsoft.OpenApi.Models;

namespace DuckpondExample.Gateway;

/// <summary>
/// The main entry point for the application.
/// </summary>
/// <param name="args">The command-line arguments.</param>
public class Program
{
    /// <summary>
    /// The main method that configures and runs the web application.
    /// </summary>
    /// <param name="args">The command-line arguments.</param>
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();
        builder.Configuration.AddJsonFile("./Configuration/gateway.config.json", optional: false, reloadOnChange: true);
        builder.Logging.AddConsole();
        builder.AddAttributedServices();
        // Add services to the container.
        builder.Services.AddSingleton(builder.Configuration.GetSection("ApiGatewaySettings").Get<ApiGatewaySettings>() ?? throw new KeyNotFoundException("Section 'ApiGatewaySettings' not found."));
        builder.Services.AddScoped<PermissionService>();

        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "Duckpond.Aspire.Gateway", Version = "v1" });
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        });

        builder.Services.AddRazorPages();
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder => builder.AllowAnyOrigin()
                                  .AllowAnyHeader()
                                  .AllowAnyMethod());
        });

        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7033") });

        builder.Services.AddHttpClient<PermissionApiClient>(client =>
        {
            client.BaseAddress = new Uri("https://localhost:7033/api/");
            client.Timeout = TimeSpan.FromMinutes(3);
        });

        var app = builder.Build();

        // Update Permissions
        using (var scope = app.Services.CreateScope())
        {
            var permissionService = scope.ServiceProvider.GetRequiredService<PermissionService>();
            await permissionService.UpdateKnownPermissions();
        }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }
        app.UseBlazorFrameworkFiles();
        app.UseStaticFiles();
        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.UseCors("AllowAllOrigins");

        // Configure the HTTP request pipeline.
        app.UseSwagger();
        app.UseSwaggerUI(setup =>
        {
            using var scope = app.Services.CreateScope();
            var apiGatewaySettings = scope.ServiceProvider.GetRequiredService<ApiGatewaySettings>();

            var knownServices = apiGatewaySettings.ApiServices;
            knownServices.ToList().ForEach(service =>
            {
                setup.SwaggerEndpoint($"/openid/{service.Value.Name}/openapi.json", service.Value.Name);
            });
        });

        app.MapControllers();
        app.MapRazorPages();
        app.MapFallbackToFile("index.html");
        await app.RunAsync();
    }
}
