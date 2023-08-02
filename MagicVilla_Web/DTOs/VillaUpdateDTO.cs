using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Web.DTOs
{
    public class VillaUpdateDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        public string Details { get; set; }
        [Required]
        public decimal Rate { get; set; }
        [Required]
        public int Sqft { get; set; }
        [Required]
        public int Occupancy { get; set; }
        [Required]
        public string ImageURL { get; set; }
        public string Amenity { get; set; }
    }
}
