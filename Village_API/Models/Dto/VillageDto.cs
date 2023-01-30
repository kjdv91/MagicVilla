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
        public string SqauerMeter { get; set; }

    }
}
