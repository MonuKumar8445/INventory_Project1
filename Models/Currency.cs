using INventory_Project1.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INventory_Project1.Models
{
    public class Currency
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(25)]
        public string Name { get; set; }
        [Required]
        [StringLength(95)]
        public string Description { get; set; }
        [ForeignKey("Currencies")]
        public int? ExchangeCurrencyId { get; set; }
        public virtual Currency Currencies { get; set; }
        [Required]
        [Column(TypeName="smallmoney")]
        [DisplayFormat(DataFormatString ="{0:0.000}",ApplyFormatInEditMode =true)]
        public decimal ExchangeRate { get; set; }

    }
}
