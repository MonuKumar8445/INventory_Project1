using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace INventory_Project1.Models
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string? Name { get; set; }
        [Required]
        [StringLength(70)]
        public string? Description { get; set; }
    }
}
