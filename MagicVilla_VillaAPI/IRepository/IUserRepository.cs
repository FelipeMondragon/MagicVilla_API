using MagicVilla_VillaAPI.DTOs;
using MagicVilla_VillaAPI.Models;

namespace MagicVilla_VillaAPI.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<LocalUser> Register (RegistrationRequestDTO registrationRequestDTO);
    }
}
