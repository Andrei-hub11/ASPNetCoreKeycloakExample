using KeycloackTest.Contracts;

namespace KeycloackTest.Services;

public interface IAccountService
{
    Task<IReadOnlyList<UserResponseDTO>> GetAllUsersAsync();
    Task<AuthResponseDTO> RegisterUserAync(UserRegisterRequestDTO requestDTO);
    Task<AuthResponseDTO> LoginUserAync(UserLoginRequestDTO request);
}
