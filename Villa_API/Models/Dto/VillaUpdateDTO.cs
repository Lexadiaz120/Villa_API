using System.ComponentModel.DataAnnotations;

namespace Villa_API.Models.Dto
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
        public double Rate { get; set; }
        public int Occupancy { get; set; }
        [Required] 
        public int Sqft { get; set; }
        [Required]
        public string imageUrl { get; set; }
        public string Amenity { get; set; }
    }
}
