using INventory_Project1.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace INventory_Project1.Models
{
    public class Product
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
        [Display(Name="Unit")]
        public int UnitId { get; set; }
        public virtual Unit Units { get; set; }
        [Required]
        [ForeignKey("Brands")]
        [Display(Name = "Brand")]
        public int BrandId { get; set; }
        public virtual Brand Brands { get; set; }
        [Required]
        [ForeignKey("Categories")]
        [Display(Name = "Category")]
        public int? CategoryId { get; set; }
        public virtual Category Categories { get; set; }
        [Required]
        [ForeignKey("ProductGroups")]
        [Display(Name = "ProductGroup")]
        public int? ProductGroupId { get; set; }
        public virtual ProductGroup ProductGroups { get; set; }
        [Required]
        [ForeignKey("ProductProfiles")]
        [Display(Name = "ProductProfile")]
        public int? ProductProfileId { get; set; }
        public virtual ProductProfile ProductProfiles { get; set; }
        public string PhotoUrl { get; set; } = "default-photo-url.jpg"; //"noimage.png.gif";
        [Display(Name = "Product Photo")]
        [NotMapped]
        public IFormFile ProductPhoto { get; set; }
    }
}
