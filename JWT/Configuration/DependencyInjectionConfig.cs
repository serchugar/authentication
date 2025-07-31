using JWT.Services.JwtAuth;
using JWT.Services.Users;

namespace JWT.Configuration;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddDependencyInjectionConfig(this IServiceCollection services)
    {
        services.AddScoped<UserRepository>();
        services.AddScoped<JwtAuthService>();
        return services;
    }
}