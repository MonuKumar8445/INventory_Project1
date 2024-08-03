using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INventory_Project1.Models
{
    public class PoDetail
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("PoHeader")]
        public int PoId { get; set; }
        public virtual PoHeader PoHeader { get; private set; }
        [Required]
        [ForeignKey("Product")]
        public string ProductCode { get; set; }
        public virtual Product Product { get; private set; }

        [Required]
        [Column(TypeName = "smallmoney")]
        public decimal Quantity { get; set; }

        [Required]
        [Column(TypeName = "smallmoney")]
        [DisplayFormat(DataFormatString = "{0:0.000}", ApplyFormatInEditMode = true)]
        public decimal FOB { get; set; } // FOB means Free On Board

        [Required]
        [Column(TypeName = "smallmoney")]
        [DisplayFormat(DataFormatString = "{0:0.000}", ApplyFormatInEditMode = true)]
        public decimal PrcInBaseCur { get; set; }
        [MaxLength(100)]
        [NotMapped]
        public string Description { get; set; }

        [MaxLength(20)]
        [NotMapped]
        public string UnitName { get; set; }

        [NotMapped]
        public bool IsDeleted { get; set; } = false;
        [NotMapped]
        public decimal Total { get; set; }
        public decimal Amounts { get; set; }
        public decimal Price { get; set; }

    }
}
