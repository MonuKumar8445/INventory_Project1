using INventory_Project1.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INnventory_Project1.Models
{
    public class ProductDetails
    {
        [Key]
        [StringLength(6)]
        public string Code { get; set; }
        [Required]
        [StringLength(75)]
        public string Name { get; set; }
        [Required]
        [StringLength(225)]
        public string Description { get; set; }
        [Required]
        [Column(TypeName = "smallmoney")]
        public decimal Cost { get; set; }
        [Required]
        [Column(TypeName = "smallmoney")]
        public decimal Price { get; set; }
        [Required]
        [ForeignKey("Units")]
        public int UnitId { get; set; }
        public virtual Unit Units { get; set; }

    }
}
