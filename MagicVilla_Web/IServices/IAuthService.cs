using MagicVilla_Web.Models.DTOs;

namespace MagicVilla_Web.IServices
{
    public interface IAuthService
    {
        Task<T> LoginAsync<T>(LoginRequestDTO objToCreate);
        Task<T> RegisterAsync<T>(UserDTO objToCreate);
    }
}
