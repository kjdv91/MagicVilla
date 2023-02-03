using System.ComponentModel.DataAnnotations;

namespace Village_API.Models.Dto
{
    public class VillageNumberDto
    {
        [Required]
        public int VillageNro { get; set; }
        [Required]
        public int VillaId { get; set; }
        public string SpecialDetails { get; set; }
    }
}
