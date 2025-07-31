using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace JWT.Configuration;

public static class JwtConfig
{
    public static IServiceCollection AddJwtConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opts =>
        {
            opts.TokenValidationParameters = new()
            {
                ValidateIssuer = true,
                ValidIssuer = configuration["JwtSettings:Issuer"],
                ValidateAudience = true,
                ValidAudience = configuration["JwtSettings:Audience"],
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Token"]!)),
                ValidateIssuerSigningKey = true,
            };
        });
        return services;
    }

    public static IApplicationBuilder UseJwtConfig(this IApplicationBuilder app)
    {
        app.UseAuthorization();
        return app;
    }
}