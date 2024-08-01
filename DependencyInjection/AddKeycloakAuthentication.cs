using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace KeycloackTest.DependencyInjection;

public static class KeycloakAuthenticationExtensions
{
    public static IServiceCollection AddKeycloakAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var keycloakSettings = new KeycloakSettings();
        configuration.GetSection("Keycloak").Bind(keycloakSettings);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.Authority = $"{keycloakSettings.AuthServerUrl}realms/{keycloakSettings.Realm}";
            options.RequireHttpsMetadata = false;
            options.Audience = keycloakSettings.Resource;
            options.IncludeErrorDetails = true;

            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = keycloakSettings.VerifyTokenAudience,
                ValidAudience = keycloakSettings.Resource,
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateLifetime = true,
            };
        });

        return services;
    }
}

public class KeycloakSettings
{
    public string Realm { get; set; }
    public string AuthServerUrl { get; set; }
    public string Resource { get; set; }
    public bool VerifyTokenAudience { get; set; }
    public Credentials Credentials { get; set; }
    public int ConfidentialPort { get; set; }
    public PolicyEnforcer PolicyEnforcer { get; set; }
}

public class Credentials
{
    public string Secret { get; set; }
}

public class PolicyEnforcer
{
    public Credentials Credentials { get; set; }
}

