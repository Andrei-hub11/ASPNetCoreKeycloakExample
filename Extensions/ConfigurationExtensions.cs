using System.Diagnostics.CodeAnalysis;

namespace KeycloackTest.Extensions;

public static class ConfigurationExtensions
{
    public static string GetRequiredValue(this IConfiguration configuration, string key)
    {
        var value = configuration[key];
        if (string.IsNullOrEmpty(value))
        {
            throw new InvalidOperationException($"Configuration key '{key}' is missing or null.");
        }
        return value;
    }
}

