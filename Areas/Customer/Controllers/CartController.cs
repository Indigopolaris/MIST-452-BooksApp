using books452.Data;
using books452.Models;
using books452.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                OrderTotal = 0
            };

            foreach(var cartItem in shoppingCartVM.CartItems)
            {
                cartItem.SubTotal = cartItem.Book.Price * cartItem.Quantity;// subtotal for each item in cart

                shoppingCartVM.OrderTotal += cartItem.SubTotal;
            }
            return View(shoppingCartVM);
        }

        public IActionResult IncrementByOne(int id)
        {
            Cart cart =_dbContext.Carts.Find(id);
            cart.Quantity++;

            _dbContext.Update(cart);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult DecrementByOne(int id)
        {
            Cart cart = _dbContext.Carts.Find(id);

            if(cart.Quantity <= 1)
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
    }
}
