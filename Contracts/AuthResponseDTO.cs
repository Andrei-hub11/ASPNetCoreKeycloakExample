namespace KeycloackTest.Contracts;

public sealed record AuthResponseDTO(UserResponseDTO User, string AccessToken, string RefreshToken);