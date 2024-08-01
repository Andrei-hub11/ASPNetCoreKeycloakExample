using KeycloackTest.Services;

namespace KeycloackTest.DependencyInjection;

public static class ServiceCollection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddHttpClient();

        return services;
    }
}
