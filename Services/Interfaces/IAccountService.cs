using KeycloackTest.Contracts;

namespace KeycloackTest.Services;

public interface IAccountService
{
    Task<IReadOnlyList<UserResponseDTO>> GetAllUsersAsync();
    Task<AuthResponseDTO> RegisterUserAsync(UserRegisterRequestDTO requestDTO);
    Task<AuthResponseDTO> LoginUserAsync(UserLoginRequestDTO request);
}
