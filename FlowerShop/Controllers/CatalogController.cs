using FlowerShop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FlowerShop.Models;
using System.Web.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;



namespace FlowerShop.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ApplicationDbContext _context;
        public CatalogController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            //SelectList categories = new SelectList(_context.Categories, "Id", "Title");
            ViewBag.Categories = _context.Categories.ToList();
            return View(await _context.Products.ToListAsync());

        }
        public async Task<IActionResult> Picture(int? id, int x, int y)
        {
            if (id == null)
            {
                return NotFound();
            }
           
            var product = await _context.Products
               .FirstOrDefaultAsync(m => m.Id == id);
            var im = new WebImage(product.Picture)
           .Resize(x, y, false, true) 
           .Crop(1, 1)
           .GetBytes();
            if (product == null)
            {
                return NotFound();
            }
            return base.File(im, "image/jpg");
        }
        /*  private bool CategoriesExists(int id)
          {
              return _context.Categories.Any(e => e.Id == id);
          }*/
    }
}
