using MagicVilla_Web.DTOs;

namespace MagicVilla_Web.IServices
{
    public interface IAuthService
    {
        Task<T> LoginAsync<T>(LoginRequestDTO objToCreate);
        Task<T> RegisterAsync<T>(RegistrationRequestDTO objToCreate);
    }
}
