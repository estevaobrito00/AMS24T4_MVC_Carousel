using AMST4.Carousel.MVC.Context;
using AMST4.Carousel.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace AMST4.Carousel.MVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDataContext _applicationDataContext;
        public CategoryController(ApplicationDataContext applicationDataContext)
        {
            _applicationDataContext = applicationDataContext;
        }
        public IActionResult CategoryList()
        {
            var categories = _applicationDataContext.Category.ToList();

            return View(categories);
        }
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddCategory(Category category)
        {
            _applicationDataContext.Category.Add(category);
            _applicationDataContext.SaveChanges();
            return RedirectToAction("CategoryList");
        }
    }
}