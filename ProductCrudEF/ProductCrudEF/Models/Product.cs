using ProductCrudEF.Models.ViewModels;

namespace ProductCrudEF.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = null!;

        public string Description { get; set; } = null!;

        public double Price { get; set; }

        public string? ImageUrl { get; set; }

        public Category Category { get; set; }

        public string CategoryId { get; set; }

        public static implicit operator Product(AddProductViewModel viewModel)
        {
            return new Product
            {
                Name = viewModel.Name,
                Description = viewModel.Description,
                CategoryId = viewModel.CategoryId,
                Price = viewModel.Price,
            };
        }
    }
}