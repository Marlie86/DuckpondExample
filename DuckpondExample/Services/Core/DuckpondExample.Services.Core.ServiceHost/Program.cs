using DuckpondExample.Services.Core.Shared.DataItems;
using DuckpondExample.Shared.Common.Hosts.Extensions;
using DuckpondExample.Shared.Common.Hosts.Repositories;
using DuckpondExample.Shared.Common.Hosts.Utilities;

using FluentMigrator.Runner;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using System.Text;

namespace DuckpondExample.Services.Core;

/// <summary>
/// The main entry point for the application.
/// Configures services and the HTTP request pipeline.
/// </summary>
/// <param name="args">The command-line arguments.</param>
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();
        builder.Logging.AddConsole();
        builder.AddAttributedServices();

        // Add database migraions.
        builder.Services.AddFluentMigratorCore()
            .ConfigureRunner(runner =>
            {
                runner.AddSqlServer()
                    .WithGlobalConnectionString(builder.Configuration.GetConnectionString("DefaultConnection"))
                    .ScanIn(typeof(Program).Assembly).For.Migrations();
            })
            .AddLogging(logBuilder => logBuilder.AddFluentMigratorConsole());

        // Mediatr
        builder.Services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        });

        // Automapper
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        // Add services to the container.
        builder.Services.AddSingleton<DapperContext>();
        builder.Services.AddSingleton<IGenericRepository<User>, GenericRepository<User>>();
        builder.Services.AddScoped<JwtUtility>();


        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        builder.Services.AddEndpointsApiExplorer();
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

        //Authentication Authorization
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration.GetValue<string>("Jwt:Issuer"),
                    ValidAudience = builder.Configuration.GetValue<string>("Jwt:Audience"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Jwt:Key") ?? "123"))
                };
            });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder => builder.AllowAnyOrigin()
                                  .AllowAnyHeader()
                                  .AllowAnyMethod());
        });

        var app = builder.Build();

        //if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage(); 
            using (var scope = app.Services.CreateScope())
            {
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
                if (runner.HasMigrationsToApplyRollback())
                {
                    try
                    {
                        var stepCount = runner.MigrationLoader.LoadMigrations().Count();
                        runner.Rollback(stepCount);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }

        // Run migrations.  
        using (var scope = app.Services.CreateScope())
        {
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            if (runner.HasMigrationsToApplyUp())
            {
                runner.MigrateUp();
            }
        }
        
        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseCors("AllowAllOrigins");
        app.MapControllers();

        app.Run();
    }
}
