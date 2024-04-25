using Microsoft.AspNetCore.Mvc;

namespace books452.Models.ViewModels
{
    public class OrderVM 
    {
        public Order Order { get; set; }
        public IEnumerable<OrderDetail> OrderDetails { get; set; }
    }
}
