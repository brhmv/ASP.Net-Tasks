using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TodoWebService.Auth;
using TodoWebService.Data;
using TodoWebService.Models.Entities;
using TodoWebService.Providers;
using TodoWebService.Services.Todo;

namespace TodoWebService;

public static class DI
{
    #region Swagger

    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(setup =>
        {
            setup.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = "My Api - V1",
                    Version = "v1",
                });

            setup.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Exapmle: \"Bearer {token}\""
            });

        });


        return services;
    }
    #endregion

    #region Authentication and Authorization

    public static IServiceCollection AddAuthenticationAndAuthorization(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddIdentity<AppUser, IdentityRole>(setup => { })
            .AddEntityFrameworkStores<TodoDbContext>();

        var jwtConfig = new JwtConfig();
        configuration.GetSection("JWT").Bind(jwtConfig);
        services.AddSingleton(jwtConfig);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, setup =>
        {
            setup.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidAudience = jwtConfig.Audience,
                ValidIssuer = jwtConfig.Issuer,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Secret))
            };
        });

        services.AddAuthorization();

        return services;
    }
    #endregion

    #region DbContext
    public static IServiceCollection AddTodoContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<TodoDbContext>(op => op.UseSqlServer(configuration.GetConnectionString("TodoConStr")).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));

        return services;
    }
    #endregion

    #region Domain Services
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<ITodoService, TodoService>();
        services.AddScoped<IJwtService, JwtService>();
        services.AddScoped<IRequestUserProvider, RequestUserProvider>();
        return services;
    }
    #endregion
}