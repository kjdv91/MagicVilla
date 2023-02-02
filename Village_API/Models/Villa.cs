using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Village_API.Models
{
    public class Villa
    {
        [Key]  //PrimaryKey
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  //se autoincrementa el id
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime EmitionCreated { get; set; }
        public string Details { get; set; }
        [Required]
        public double Fee { get; set; }
        public int Capacity { get; set; }
        public double SquareMetters { get; set; }
        public string ImageUrl { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
