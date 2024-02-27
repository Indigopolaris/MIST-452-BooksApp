using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace books452.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [DisplayName("Book Title")]
        public string BookTitle { get; set; }

        public string Author { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public string? ImgUrl { get; set; }

        public int CategoryId { get; set; } //FK
        [ForeignKey("CategoryId")]
        public Category? category { get; set; }//nav property
    }
}
