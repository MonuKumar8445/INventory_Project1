using System.ComponentModel.DataAnnotations;

namespace INventory_Project1.Models
{
    public class Supplier
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(6)]
        public string Code { get; set; } = ""; // Assigns an empty string to the variable myString
        [Required]
        [MaxLength(60)]
        public string Name { get; set; } = "";
        [Required]
        public string EmailId { get; set; } = "";
        [Required]
        public string PhoneNo { get; set; } = "";
        [Required]
        public string Address { get; set; } = "";
    }
}
