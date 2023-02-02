using System.ComponentModel.DataAnnotations;

namespace Village_API.Models.Dto
{
    public class VillageDto
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)] //maximo 30 caracteres
        public string Name { get; set; }
        public int Capacity { get; set; }
        public double SquareMetters { get; set; }
        public string Details { get; set; }
        public string ImageUrl { get; set; }
        public DateTime UpdateDate { get; set; }

    }
}
