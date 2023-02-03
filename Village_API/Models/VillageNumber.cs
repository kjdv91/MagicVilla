using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Village_API.Models
{
    public class VillageNumber
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.None)]  //ingresa el id el user
        public int VillageNro { get; set; }
        [Required]
        public int VillaId { get; set; }  //propieda relacional
        [ForeignKey("VillaId")] //relacion
        public Villa Villa { get; set; }  //navegacion

        public string SpecialDetails { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdateDate { get; set; }


    }
}
