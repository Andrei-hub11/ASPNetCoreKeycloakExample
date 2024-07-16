namespace KeycloackTest.Contracts;

public sealed record UserRegisterRequestDTO(string UserName, string Email, string Password);
