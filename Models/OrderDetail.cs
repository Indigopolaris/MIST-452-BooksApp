using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace books452.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }

        public int OrderId { get; set; }

        public int BookId { get; set; }

        [ValidateNever]
        [ForeignKey("BookId")]
        public Book Book { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }  


    }
}
