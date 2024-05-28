using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AMST4.Carousel.MVC.Context;
using AMST4.Carousel.MVC.Models;

namespace AMST4.Carousel.MVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDataContext _context;

        public CategoryController(ApplicationDataContext context)
        {
            _context = context;
        }

        public IActionResult CategoryList()
        {
            var category = _context.Category.ToList();
            return View(category);
        }

        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _context.Category
                .FirstOrDefault(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        public IActionResult AddCategory()
        {
            return View();
        }

    
        [HttpPost]
        
        public IActionResult AddCategory(Category category, IFormFile image)
        {
                var fileName = Guid.NewGuid().ToString() + image.FileName;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Category", fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    image.CopyTo(stream);
                }
                category.Id = Guid.NewGuid();
                category.ImageUrl = filePath;
                _context.Add(category);
                _context.SaveChanges();
                return RedirectToAction("CategoryList");
        }

        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _context.Category.Find(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        
        [HttpPost]
        public IActionResult Edit(Guid id, Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("CategoryList");
            }
            return View(category);
        }

       
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = _context.Category
                .FirstOrDefault(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(Guid id)
        {
            var category = _context.Category.Find(id);
            if (category != null)
            {
                _context.Category.Remove(category);
            }

            _context.SaveChanges();
            return RedirectToAction("CategoryList");
        }

        private bool CategoryExists(Guid id)
        {
            return _context.Category.Any(e => e.Id == id);
        }
    }
}
