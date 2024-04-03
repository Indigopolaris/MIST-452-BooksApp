using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace books452.Models
{
    public class Order
    {

        public int OrderId { get; set; }

        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")] // apply to the one above^
        [ValidateNever]
        public ApplicationUser ApplicationUser { get; set; }

        

        public string CustomerName { get; set; }

        public string? StreetAddress { get; set; }

        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set; }

        public string? Phone { get; set; }

        public decimal OrderTotal { get; set; }

        public string OrderStatus { get; set; }

        public string PaymentStatus { get; set; }
        public DateOnly OrderDate { get; set; }

    }
}
