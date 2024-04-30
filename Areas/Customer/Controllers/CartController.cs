using books452.Data;
using books452.Models;
using books452.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.Checkout;
using System.Security.Claims;

namespace books452.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {
        private BooksDBContext _dbContext;
        public CartController(BooksDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);//fetch user id

            var cartItemsList = _dbContext.Carts.Where(c => c.UserId == userId).Include(c => c.Book);

            ShoppingCartVM shoppingCartVM = new ShoppingCartVM
            {
                CartItems = cartItemsList,
                Order = new Order()
            };

            foreach (var cartItem in shoppingCartVM.CartItems)
            {
                cartItem.SubTotal = cartItem.Book.Price * cartItem.Quantity;// subtotal for each item in cart

                shoppingCartVM.Order.OrderTotal += cartItem.SubTotal;
            }
            return View(shoppingCartVM);
        }

        public IActionResult IncrementByOne(int id)
        {
            Cart cart = _dbContext.Carts.Find(id);
            cart.Quantity++;

            _dbContext.Update(cart);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult DecrementByOne(int id)
        {
            Cart cart = _dbContext.Carts.Find(id);

            if (cart.Quantity <= 1)
            {
                _dbContext.Carts.Remove(cart);
                _dbContext.SaveChanges();
            }
            else
            {
                cart.Quantity--;
                _dbContext.Update(cart);
            }

            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(int id)
        {
            Cart cart = _dbContext.Carts.Find(id);

            _dbContext.Carts.Remove(cart);

            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult ReviewOrder()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); //fetches user id
            var cartItemsList = _dbContext.Carts.Where(c => c.UserId == userId).Include(c => c.Book); //query the cart by user id, include books in cart

            ShoppingCartVM shoppingCartVM = new ShoppingCartVM
            {
                CartItems = cartItemsList,
                Order = new Order()
            };

            foreach (var cartItem in shoppingCartVM.CartItems)
            {
                cartItem.SubTotal = cartItem.Book.Price * cartItem.Quantity;// subtotal for each item in cart

                shoppingCartVM.Order.OrderTotal += cartItem.SubTotal;
            }

            shoppingCartVM.Order.ApplicationsUser = _dbContext.ApplicationUsers.Find(userId);
            shoppingCartVM.Order.CustomerName = shoppingCartVM.Order.ApplicationsUser.Name;
            shoppingCartVM.Order.StreetAddress = shoppingCartVM.Order.ApplicationsUser.StreetAddress;
            shoppingCartVM.Order.City = shoppingCartVM.Order.ApplicationsUser.City;
            shoppingCartVM.Order.State = shoppingCartVM.Order.ApplicationsUser.State;
            shoppingCartVM.Order.PostalCode = shoppingCartVM.Order.ApplicationsUser.PostalCode;
            shoppingCartVM.Order.Phone = shoppingCartVM.Order.ApplicationsUser.PhoneNumber;

            return View(shoppingCartVM);
        }

        [HttpPost]
        [ActionName("ReviewOrder")]
        public IActionResult ReviewOrderPOST(ShoppingCartVM shoppingCartVM)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); //fetches user id
            var cartItemsList = _dbContext.Carts.Where(c => c.UserId == userId).Include(c => c.Book); //query the cart by user id, include books in cart

            shoppingCartVM.CartItems = cartItemsList;

            foreach (var cartItem in shoppingCartVM.CartItems)
            {
                cartItem.SubTotal = cartItem.Book.Price * cartItem.Quantity;// subtotal for each item in cart

                shoppingCartVM.Order.OrderTotal += cartItem.SubTotal;
            }

            shoppingCartVM.Order.ApplicationsUser = _dbContext.ApplicationUsers.Find(userId);
            shoppingCartVM.Order.CustomerName = shoppingCartVM.Order.ApplicationsUser.Name;
            shoppingCartVM.Order.StreetAddress = shoppingCartVM.Order.ApplicationsUser.StreetAddress;
            shoppingCartVM.Order.City = shoppingCartVM.Order.ApplicationsUser.City;
            shoppingCartVM.Order.State = shoppingCartVM.Order.ApplicationsUser.State;
            shoppingCartVM.Order.PostalCode = shoppingCartVM.Order.ApplicationsUser.PostalCode;
            shoppingCartVM.Order.Phone = shoppingCartVM.Order.Phone;

            shoppingCartVM.Order.OrderDate = DateOnly.FromDateTime(DateTime.Now);
            shoppingCartVM.Order.OrderStatus = "pending";
            shoppingCartVM.Order.PaymentStatus = "pending";

            _dbContext.Orders.Add(shoppingCartVM.Order);
            _dbContext.SaveChanges();

            foreach (var eachCartItem in shoppingCartVM.CartItems)
            {
                OrderDetail orderDetail = new OrderDetail 
                {
                    OrderId = shoppingCartVM.Order.OrderID, 
                    BookId = eachCartItem.BookId, 
                    Quantity = eachCartItem.Quantity,
                    Price = eachCartItem.Book.Price
                };
                _dbContext.OrderDetails.Add(orderDetail);
            }
            _dbContext.SaveChanges();
            //StripeConfiguration.ApiKey = "sk_test_51PB5h6FuVrSKAs92rrs3nY8NP7JhbcJ05VDrK1tQDpnDcMtwM3GyIVoeEeJ6wyguzK8q6JOy6kIUJZGrHkt5xPwN00eTUKvZj0";
            var options = new Stripe.Checkout.SessionCreateOptions
            {
                SuccessUrl = "https://localhost:7165/" + $"customer/cart/orderconfirmation?id={shoppingCartVM.Order.OrderID}",
                CancelUrl = "https://localhost:7165/" + "customer/cart/index",
                LineItems = new List<Stripe.Checkout.SessionLineItemOptions>
    {
        new Stripe.Checkout.SessionLineItemOptions
        {
            //Price = "price_1MotwRLkdIwHu7ixYcPLm5uZ",
            //Quantity = 2,
        },
    },
                Mode = "payment",
            };
            foreach(var eachcartItems in shoppingCartVM.CartItems)
            {
                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)eachcartItems.Book.Price, //20.99 -> 2099
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = eachcartItems.Book.BookTitle
                        }
                    },
                    Quantity = eachcartItems.Quantity,
                };

                options.LineItems.Add(sessionLineItem); 
            }
            var service = new Stripe.Checkout.SessionService();
            Session session = service.Create(options);
            shoppingCartVM.Order.SessionID = session.Id;

            _dbContext.SaveChanges();

            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
            //return RedirectToAction("OrderConfirmation", new { id = shoppingCartVM.Order.OrderID });
        }

        public IActionResult OrderConfirmation(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            List<Cart> listOfCartItems = _dbContext.Carts.ToList().Where(c => c.UserId == userId).ToList();

            _dbContext.Carts.RemoveRange(listOfCartItems);
            _dbContext.SaveChanges();
            return View(id);
        }
    }
}
