using books452.Data;
using books452.Models;
using books452.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//add-migration AddingAdditionOrderDetailToOrderTable
namespace books452.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        public OrderVM orderVM { get; set; }
        private BooksDBContext _dbContext;

        public OrderController(BooksDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            IEnumerable<Order> listOfOrders = _dbContext.Orders.Include(o => o.ApplicationsUser);


            return View(listOfOrders);
        }
        public IActionResult Details(int id)
        {
            Order order = _dbContext.Orders.Find(id); 
           
            _dbContext.Entry(order).Reference(o => o.ApplicationUserID).Load();
            IEnumerable<OrderDetail> orderDetails = _dbContext.OrderDetails.Where(od => od.OrderId == id).Include(od => od.Book);

            OrderVM orderVM = new OrderVM
            {
                Order = order,
                OrderDetails = orderDetails
            };

            return View(orderVM);
        }
        [HttpPost]
        public IActionResult UpdateOrderInformation()
        {
            Order orderFromDB = _dbContext.Orders.Find(orderVM.Order.OrderID);
            orderFromDB.CustomerName = orderVM.Order.CustomerName;
            orderFromDB.StreetAddress = orderVM.Order.StreetAddress;
            orderFromDB.City = orderVM.Order.City;
            orderFromDB.State = orderVM.Order.State;
            orderFromDB.PostalCode = orderVM.Order.PostalCode;
            orderFromDB.Phone = orderVM.Order.Phone;

            //if(!string.IsNullOrEmpty(orderVM.Order.ShippingDate.ToString()))
            //{
            //    orderFromDB.ShippingDate = orderVM.Order.ShippingDate;
            //}

            //if (!string.IsNullOrEmpty(orderVM.Order.TrackingNumber))
            //{
            //    orderFromDB.TrackingNumber = orderVM.Order.TrackingNumber;
            //}

            //if (!string.IsNullOrEmpty(orderVM.Order.Carrier))
            //{
            //    orderFromDB.Carrier = orderVM.Order.Carrier;
            //}

            orderFromDB.ShippingDate = orderVM.Order.ShippingDate;
            orderFromDB.TrackingNumber = orderVM.Order.TrackingNumber;
            orderFromDB.Carrier = orderVM.Order.Carrier;
            orderFromDB.OrderStatus = orderVM.Order.OrderStatus;

            _dbContext.Orders.Update(orderFromDB);
            _dbContext.SaveChanges();
            return RedirectToAction("Details", new { id = orderFromDB.OrderID });
        }


        public IActionResult ProcessOrder()
        {
            Order order = _dbContext.Orders.Find(orderVM.Order.OrderID); 

            order.OrderStatus = "Processing";

            order.ShippingDate = DateOnly.FromDateTime(DateTime.Now).AddDays(7);

            order.Carrier = "USPS";

            _dbContext.Orders.Update(order); 

            _dbContext.SaveChanges();

            return RedirectToAction("Details", new { id = order.OrderID });


        }

        public IActionResult CompleteOrder()
        {
            Order order = _dbContext.Orders.Find(orderVM.Order.OrderID);

            order.OrderStatus = "Shipped and Completed";

            order.ShippingDate = DateOnly.FromDateTime(DateTime.Now);

            _dbContext.Orders.Update(order);

            _dbContext.SaveChanges();

            return RedirectToAction("Details", new { id = order.OrderID });
        }
    }
}
