using books452.Data;
using books452.Models;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Experimental.ProjectCache;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace books452.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private BooksDBContext _dbContext;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, BooksDBContext booksDBContext)
        {
            _logger = logger;
            _dbContext = booksDBContext;
        }

        public IActionResult Index()
        {
            var listOfBooks = _dbContext.Books.Include(c => c.category);
            return View(listOfBooks.ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Details()
        {
            Book book = _dbContext.Books.Find(id);
            _dbContext.Books.Entry(book).Reference(b => b.category).Load();

            var Cart = new cart
            {
                BookId = Id,
                Book = book,
                Quantity = 1

            };
            return View(cart);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Details(Cart cart)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            cart.UserId = userId;

            _dbContext.Cart.Add(cart);
            _dbContext.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
