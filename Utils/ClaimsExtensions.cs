using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace KeycloackTest.Utils;

public static class ClaimsExtensions
{
    public static bool HasRole(this ClaimsPrincipal user, string roleName)
    {
        var scopeClaim = user.FindFirst(claim => claim.Type == "scope");
        if (scopeClaim != null && scopeClaim.Value.Split(' ').Any(s => s.Equals("keycloak-scope", StringComparison.OrdinalIgnoreCase)))
        {
            var resourceAccessClaim = user.FindFirst(claim => claim.Type == "resource_access");
            if (resourceAccessClaim != null)
            {
                var resourceAccess = JObject.Parse(resourceAccessClaim.Value);
                var clientRoles = resourceAccess["keycloak-client"]?["roles"]?.Select(r => r.ToString());
                if (clientRoles != null && clientRoles.Contains(roleName, StringComparer.OrdinalIgnoreCase))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
