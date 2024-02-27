using books452.Data;
using books452.Models;
using Microsoft.AspNetCore.Mvc;

namespace books452.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private BooksDBContext _dbContext;

        public CategoryController(BooksDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()// fetch obj
        {
            var listofCategories = _dbContext.Categories.ToList();

            return View(listofCategories);
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Create(Category categoryObj)
        {
            if (categoryObj.Name != null && categoryObj.Name.ToLower() == "test")
            {
                ModelState.AddModelError("Name", "Category name cannot be 'test'");
            }
            if (categoryObj.Name == categoryObj.Description)
            {
                ModelState.AddModelError("Description", "Category name and description cannot be the same");
            }
            if (ModelState.IsValid) //makesure the model is valid (all fields are filled in)
            {
                _dbContext.Categories.Add(categoryObj);
                _dbContext.SaveChanges();

                RedirectToAction("Index", "Category");
            }
            return View(categoryObj);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Category category = _dbContext.Categories.Find(id);
            return View(category);
        }

        [HttpPost]
        public IActionResult Edit(int id, [Bind("CategoryId, Name, Description")] Category categoryObj)
        {
            if (ModelState.IsValid)
            {
                _dbContext.Categories.Add(categoryObj);
                _dbContext.SaveChanges();

                RedirectToAction("Index", "Category");
            }
            return View(categoryObj);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Category category = _dbContext.Categories.Find(id);
            return View(category);
        }

        [HttpPost]
        [ActionName("Delete")]
        public IActionResult DeletePOST(int id)
        {
            Category categoryObj = _dbContext.Categories.Find(id);

            _dbContext.Categories.Remove(categoryObj);
            _dbContext.SaveChanges();

            return RedirectToAction("Index", "Category");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            Category categoryObj = _dbContext.Categories.Find(id);

            return View(categoryObj);
        }
    }
}
