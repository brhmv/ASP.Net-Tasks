using System.ComponentModel.DataAnnotations;

namespace ProductCrudEF.Models.ViewModels
{
    public class AddProductViewModel
    {
        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(20)]
        public string Description { get; set; }

        [Required]
        public string CategoryId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public double Price { get; set; }

     
        [Required(ErrorMessage = "please select an image.")]
        [Display(Name = "product image")]
        public IFormFile ImageFile { get; set; }
    }
}