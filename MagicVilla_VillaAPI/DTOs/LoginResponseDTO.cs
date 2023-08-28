using MagicVilla_VillaAPI.Models;

namespace MagicVilla_VillaAPI.DTOs
{
    public class LoginResponseDTO
    {
        public UserDTO User { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
