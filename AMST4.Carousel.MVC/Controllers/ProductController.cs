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
    public class ProductController : Controller
    {
        private readonly ApplicationDataContext _context;

        public ProductController(ApplicationDataContext context)
        {
            _context = context;
        }

       
        public IActionResult ProductList()
        {
            var product = _context.Product.ToList();
            return View(product);
        }

        
        public IActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _context.Product
                .FirstOrDefault(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public IActionResult AddProduct()
        {
            return View();
        }

        
        [HttpPost]

        public IActionResult AddProduct(Product product, IFormFile image)
        {
            var fileName = Guid.NewGuid().ToString() + image.FileName;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Product", fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                image.CopyTo(stream);
            }
            product.Id = Guid.NewGuid();
            product.ImageUrl = filePath;
            _context.Add(product);
            _context.SaveChanges();
            return RedirectToAction("ProductList");
        }


        public IActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _context.Product.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

      
        [HttpPost]
        
        public IActionResult Edit(Guid id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("ProductList");
            }
            return View(product);
        }

        
        public IActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _context.Product
                .FirstOrDefault(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        
        [HttpPost, ActionName("Delete")]
        
        public IActionResult DeleteConfirmed(Guid id)
        {
            var product = _context.Product.Find(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

             _context.SaveChanges();
            return RedirectToAction("ProductList");
        }

        private bool ProductExists(Guid id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
