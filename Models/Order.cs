using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace books452.Models
{
    public class Order
    {

        public int OrderID { get; set; }
        public string ApplicationUserID { get; set; }
        [ForeignKey("ApplicationUserID")]
        [ValidateNever]

        public ApplicationUser ApplicationsUser { get; set; } //nav prop

        public DateOnly OrderDate { get; set; }

        public string CustomerName { get; set; }

        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Phone { get; set; }

        public decimal OrderTotal { get; set; }

        public string OrderStatus { get; set; }

        public string PaymentStatus { get; set; }

        public string? Carrier { get; set; }

        public string? TrackingNumber { get; set; }

        public DateOnly? ShippingDate { get; set; }


        //stripe will give us a session ID
        public string? SessionID { get; set; }

        public string? PaymentIntentID { get; set; }

    }
}
