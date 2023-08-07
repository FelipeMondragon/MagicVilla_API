using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.DTOs;
using MagicVilla_VillaAPI.IRepository;
using MagicVilla_VillaAPI.Models;

namespace MagicVilla_VillaAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public bool IsUniqueUser(string username)
        {
            throw new NotImplementedException();
        }

        public Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            throw new NotImplementedException();
        }

        public Task<LocalUser> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            throw new NotImplementedException();
        }
    }
}
