using books452.Data;
using books452.Models;
using books452.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace books452.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookController : Controller
    {
        private BooksDBContext _dbContext;
        private IWebHostEnvironment _environment;
        public BookController(BooksDBContext dbContext, IWebHostEnvironment environment)
        {
            _dbContext = dbContext;//passes the dbcontext obj instance var
            _environment = environment;
        }
        public IActionResult Index()
        {
            var listofBooks = _dbContext.Books.ToList();

            return View(listofBooks);
        }
        //end student part

        [HttpGet]
        public IActionResult Create()
        {
            IEnumerable<SelectListItem> listOfCategories = _dbContext.Categories.ToList().Select(o => new SelectListItem
            {
                Text = o.Name,
                Value = o.CategoryId.ToString()
            }); //projection - allows projection of a category obj to selectlistitem where name of category is used as Text and ID is value

            // 1) ViewBag
            //ViewBag.ListOfCategories = listOfCategories;

            // 2) ViewData
            //ViewData["listOfCatagoriesVD"] = listOfCategories;

            // 3) ViewModel, used to support complex view 
            BookWithCategoriesVM bookWithCategoriesVMobj = new BookWithCategoriesVM();

            bookWithCategoriesVMobj.Book = new Book();
            bookWithCategoriesVMobj.ListOfCategories = listOfCategories;

            return View(bookWithCategoriesVMobj);
        }

        [HttpPost]
        public IActionResult Create(BookWithCategoriesVM bookWithCategoriesVMobj, IFormFile imgFile)
        {
            if (ModelState.IsValid)
            {
                string wwwrootPath = _environment.WebRootPath;
                if (imgFile != null)
                {
                    using (var fileStream = new FileStream(Path.Combine
                        (wwwrootPath, @"Images\bookImages\" + imgFile.FileName), FileMode.Create))
                    {
                        imgFile.CopyTo(fileStream);//saves the file into the specified folder
                    }
                    bookWithCategoriesVMobj.Book.ImgUrl = @"\Images\bookImages\" + imgFile.FileName; // @ tells compilier its a string can also do double \\
                }

                _dbContext.Books.Add(bookWithCategoriesVMobj.Book);
                _dbContext.SaveChanges();

                return RedirectToAction("Index", "Book");
            }
            return View(bookWithCategoriesVMobj);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Book book = _dbContext.Books.Find(id);

            IEnumerable<SelectListItem> listOfCategories = _dbContext.Categories.ToList().Select(o => new SelectListItem
            {
                Text = o.Name,
                Value = o.CategoryId.ToString()
            });

            BookWithCategoriesVM bookWithCategoriesVM = new BookWithCategoriesVM();

            bookWithCategoriesVM.Book = book;

            bookWithCategoriesVM.ListOfCategories = listOfCategories;

            return View(bookWithCategoriesVM);

        }
    }
}
