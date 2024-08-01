using KeycloackTest.Contracts;
using KeycloackTest.DTOMappers;
using KeycloackTest.Extensions;
using KeycloackTest.Models;
using KeycloackTest.Domain.Utils;
using KeycloackTest.Domain.Utils.Exceptions;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using KeycloackTest.Utils;

namespace KeycloackTest.Services;

public class AccountService : IAccountService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public AccountService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<IReadOnlyList<UserResponseDTO>> GetAllUsersAsync()
    {
        var users = await GetUsersAsync();

        return users.ToDTO();
    }

    public async Task<AuthResponseDTO> RegisterUserAsync(UserRegisterRequestDTO request)
    {
        var tokenResponse = await GetAdminTokenAsync();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

        var user = new
        {
            username = request.UserName,
            email = request.Email,
            enabled = true,
            credentials = new[]
            {
            new
            {
                type = "password",
                value = request.Password,
                temporary = false
            }
        },
        };

        var json = JsonConvert.SerializeObject(user);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"http://localhost:8080/admin/realms/keycloacktest/users", content);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new BadHttpRequestException(error);
        }

        var newUser = await GetUserByNameAsync(request.UserName);

        await AddUserToGroupByNameAsync(newUser.Id, "Users");
        var groupId = await GetGroupIdByNameAsync("Users");
        await AddGroupRoleToUserAsync(newUser.Id, groupId, "User");

        var userToken = await GetUserTokenAsync(request.UserName, request.Password);

        return new AuthResponseDTO(newUser.ToDTO(), AccessToken: userToken.AccessToken,
            RefreshToken: userToken.RefreshToken);
    }

    public async Task<AuthResponseDTO> LoginUserAsync(UserLoginRequestDTO request)
    {
        var userToken = await GetUserTokenAsync(request.Email, request.Password);

        var user = await GetUserByEmailAsync(request.Email);

        return new AuthResponseDTO(user.ToDTO(), AccessToken: userToken.AccessToken,
            RefreshToken: userToken.RefreshToken);
    }

    private async Task<IEnumerable<User>> GetUsersAsync()
    {
        var apiUrl = "http://localhost:8080/admin/realms/keycloacktest/users";

        var tokenResponse = await GetAdminTokenAsync();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

        var response = await _httpClient.GetAsync(apiUrl);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Failed to retrieve user details: {response.StatusCode}, {error}");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();

        var users = JsonConvert.DeserializeObject<List<User>>(jsonResponse);

        if (users == null || users.Count == 0)
        {
            throw new NotFoundException("User not found");
        }

        return users;
    }

    private async Task<User> GetUserByNameAsync(string username)
    {
        var apiUrl = $"http://localhost:8080/admin/realms/keycloacktest/users/?username={username}";

        var tokenResponse = await GetAdminTokenAsync();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

        var response = await _httpClient.GetAsync(apiUrl);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Failed to retrieve user details: {response.StatusCode}, {error}");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();

        // However, it will return a unique user.
        var users = JsonConvert.DeserializeObject<List<User>>(jsonResponse);

        if (users == null || users.Count == 0)
        {
            throw new NotFoundException("User not found");
        }

        return users.First();
    }

    private async Task<User> GetUserByEmailAsync(string email)
    {
        var apiUrl = $"http://localhost:8080/admin/realms/keycloacktest/users/?email={email}";

        var tokenResponse = await GetAdminTokenAsync();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

        var response = await _httpClient.GetAsync(apiUrl);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Failed to retrieve user details: {response.StatusCode}, {error}");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();

        // However, it will return a unique user.
        var users = JsonConvert.DeserializeObject<List<User>>(jsonResponse);

        if (users == null || users.Count == 0)
        {
            throw new NotFoundException("User not found");
        }

        return users.First();
    }

    private async Task<KeycloakToken> GetAdminTokenAsync()
    {
        var formData = new Dictionary<string, string>
        {
            { "client_id", _configuration.GetRequiredValue("UserKeycloakAdmin:client_id")},
            { "client_secret", _configuration.GetRequiredValue("UserKeycloakAdmin:client_secret") },
            { "grant_type", "password" },
            { "username", _configuration.GetRequiredValue("UserKeycloakAdmin:username") },
            { "password", _configuration.GetRequiredValue("UserKeycloakAdmin:password") }
        };

        var tokenEndpoint = _configuration.GetRequiredValue("UserKeycloakAdmin:TokenEndpoint");

        var content = new FormUrlEncodedContent(formData);
        var response = await _httpClient.PostAsync(tokenEndpoint, content);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Request failed: {response.StatusCode}, {error}");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();

        var tokenResponse = JsonConvert.DeserializeObject<KeycloakToken>(jsonResponse);

        ThrowHelper.ThrowIfNull(tokenResponse);

        return tokenResponse;
    }

    private async Task<KeycloakToken> GetUserTokenAsync(string username, string password)
    {
        var formData = new Dictionary<string, string>
    {
        { "client_id", _configuration.GetRequiredValue("UserKeycloakClient:client_id")},
        { "client_secret", _configuration.GetRequiredValue("UserKeycloakClient:client_secret") },
        { "grant_type", "password" },
        { "username", username },
        { "password", password }
    };

        var tokenEndpoint = _configuration.GetRequiredValue("UserKeycloakClient:TokenEndpoint");

        var content = new FormUrlEncodedContent(formData);
        var response = await _httpClient.PostAsync(tokenEndpoint, content);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Request failed: {response.StatusCode}, {error}");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var tokenResponse = JsonConvert.DeserializeObject<KeycloakToken>(jsonResponse);

        ThrowHelper.ThrowIfNull(tokenResponse);

        return tokenResponse;
    }

    private async Task AddUserToGroupAsync(string userId, string groupId)
    {
        var tokenResponse = await GetAdminTokenAsync();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

        var addGroupUrl = $"http://localhost:8080/admin/realms/keycloacktest/users/{userId}/groups/{groupId}";

        var response = await _httpClient.PutAsync(addGroupUrl, null);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Failed to add user to group: {response.StatusCode}, {error}");
        }
    }

    private async Task<string> GetGroupIdByNameAsync(string groupName)
    {
        var tokenResponse = await GetAdminTokenAsync();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

        var groupUrl = $"http://localhost:8080/admin/realms/keycloacktest/groups?search={groupName}";

        var response = await _httpClient.GetAsync(groupUrl);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Failed to retrieve groups: {response.StatusCode}, {error}");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var groups = JsonConvert.DeserializeObject<List<GroupResponseDTO>>(jsonResponse);

        var group = groups?.FirstOrDefault(g => g.Name.Equals(groupName, StringComparison.OrdinalIgnoreCase));

        return group?.Id ?? throw new NotFoundException("Group not found");
    }

    private async Task AddUserToGroupByNameAsync(string userId, string groupName)
    {
        var groupId = await GetGroupIdByNameAsync(groupName);
        await AddUserToGroupAsync(userId, groupId);
    }

    private async Task<List<UserResponseDTO>> GetUsersInGroupAsync(string groupId)
    {
        var tokenResponse = await GetAdminTokenAsync();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

        var usersUrl = $"http://localhost:8080/admin/realms/keycloacktest/groups/{groupId}/members";

        var response = await _httpClient.GetAsync(usersUrl);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Failed to retrieve users in group: {response.StatusCode}, {error}");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();

        var users = JsonConvert.DeserializeObject<List<UserResponseDTO>>(jsonResponse);

        ThrowHelper.ThrowIfNull(users);

        return users;
    }

    private async Task<List<RoleMappingDTO>> GetRolesByGroupIdAsync(string groupId)
    {
        var tokenResponse = await GetAdminTokenAsync();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

        var roleUrl = $"http://localhost:8080/admin/realms/keycloacktest/groups/{groupId}/role-mappings";

        var response = await _httpClient.GetAsync(roleUrl);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Failed to retrieve roles for group {groupId}: {response.StatusCode}, {error}");
        }

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var clientMappingsResponse = JsonConvert.DeserializeObject<ClientMappingsResponseDTO>(jsonResponse);

        if (clientMappingsResponse?.ClientMappings == null || !clientMappingsResponse.ClientMappings.ContainsKey("keycloak-client"))
        {
            throw new Exception($"Client keycloak-client not found or mappings empty.");
        }

        var mappings = clientMappingsResponse.ClientMappings["keycloak-client"].Mappings;

        return mappings;
    }


    public async Task AddGroupRoleToUserAsync(string userId, string groupId, string roleName)
    {
        var roles = await GetRolesByGroupIdAsync(groupId);

        var role = roles.FirstOrDefault(r => r.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));

        if (role == null)
        {
            throw new Exception($"Role '{roleName}' not found in group '{groupId}'");
        }

        await AddRoleToUserAsync(userId, role);
    }


    private async Task AddRoleToUserAsync(string userId, RoleMappingDTO role)
    {
        var tokenResponse = await GetAdminTokenAsync();

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);

        var addRoleUrl = $"http://localhost:8080/admin/realms/keycloacktest/users/{userId}/role-mappings/clients/{role.ContainerId}";
        var content = new StringContent(JsonConvert.SerializeObject(new[] { new { id = role.Id, name = role.Name } }), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(addRoleUrl, content);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Failed to add role to user: {response.StatusCode}, {error}");
        }
    }
}
