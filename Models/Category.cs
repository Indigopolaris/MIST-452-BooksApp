using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace books452.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [DisplayName("Category Name: ")]
        [Required(ErrorMessage = "The Category name MUST be provided")]
        public string? Name { get; set; }
        //? makes it nullable

        [DisplayName("Category Description: ")]
        [Required(ErrorMessage = "The Category description MUST be provided")]
        public string? Description { get; set; }

    }
}
