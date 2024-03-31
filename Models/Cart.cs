using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace books452.Models
{
    public class Cart
    {
        public int CartId { get; set; }

        public int BookId { get; set; }

        [ForeignKey("BookId")]
        [ValidateNever]
        public Book Book { get; set; }// nav prop

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }//nav prop

        public int Quantity { get; set; }
    }
}
