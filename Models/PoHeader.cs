using INventory_Project1.Migrations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INventory_Project1.Models
{
    public class PoHeader
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(6)]
        public string PoNumber { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime PoDate { get; set; } = DateTime.Now.Date;
        [Required]
        [ForeignKey("Supplier")]
        public int SupplierId { get; private set; }
        public virtual Supplier Supplier { get; set; }
        [Required]
        [ForeignKey("BaseCurrency")]
        public int BaseCurrencyId { get; set; }
        public virtual Currency BaseCurrency { get; set; }
        [Required]
        [ForeignKey("PoCurrency")]
        public int PoCurrencyId { get; set; }
        public virtual Currency PoCurrency { get; set; }
        [Required]
        [Column(TypeName = "smallmoney")]
        [DisplayFormat(DataFormatString = "{0:0.000}", ApplyFormatInEditMode = true)]
        public decimal ExchangeRate { get; set; }
        [Required]
        [Column(TypeName = "smallmoney")]
        public decimal DiscountPercent { get; set; }
        [Required]
        [MaxLength(15)]
        public string QuotationNo { get; set; }
        [Required]
        [DataType(DataType.Date)]
        public DateTime QuotationDate { get; set; } = DateTime.Now.Date;
   
        [MaxLength(500)]
        public string PaymentTerms { get; set; }
        [Required]
        [MaxLength(500)]
        public string Remarks { get; set; }
        public virtual List<PoDetail> PoDetails { get; set;} = new List<PoDetail> ();   
    }
}
