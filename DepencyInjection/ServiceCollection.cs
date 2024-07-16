using KeycloackTest.Services;

namespace KeycloackTest.DepencyInjection;

public static class ServiceCollection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, AccountService>();
        services.AddHttpClient();

        return services;
    }
}
