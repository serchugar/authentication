using JWT.Services.Auth;
using JWT.Services.Users;

namespace JWT.Configuration;

public static class DependencyInjectionConfig
{
    public static IServiceCollection AddDependencyInjectionConfig(this IServiceCollection services)
    {
        services.AddScoped<UserRepository>();
        services.AddScoped<AuthService>();
        return services;
    }
}