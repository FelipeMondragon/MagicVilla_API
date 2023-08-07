using MagicVilla_VillaAPI.Models;

namespace MagicVilla_VillaAPI.DTOs
{
    public class LoginResponseDTO
    {
        public LocalUser User { get; set; }
        public string Token { get; set; }
    }
}
