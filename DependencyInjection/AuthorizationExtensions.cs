using KeycloackTest.Utils;

namespace KeycloackTest.DependencyInjection;

public static class AuthorizationExtensions
{
    public static void AddKeycloakPolicy(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("Admin", policy =>
                policy.RequireAssertion(context => context.User.HasRole("Admin")));
        });
    }
}

