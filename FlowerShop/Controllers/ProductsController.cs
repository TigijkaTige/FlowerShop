using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlowerShop.Data;
using FlowerShop.Models;
using System.Web;
using System.IO;
using FlowerShop.ViewModels;
using System.Net;
using System.IO.Compression;
using System.Web.Helpers;
using Microsoft.AspNetCore.Authorization;


namespace FlowerShop.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "Administrator")]
        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        [Authorize(Roles = "Administrator")]
        // GET: Products/Create
        public IActionResult Create()
        {
          
            SelectList categories = new SelectList(_context.Categories, "Id", "Title");
            ViewBag.Categories = categories;
            return View();
        }
        [Authorize(Roles = "Administrator")]
        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Cost,FileName,Description,CategoryId")] Product product, IFormFile Picture)//ProductViewModel viewProduct)
        {
            if (ModelState.IsValid)
            {
                product.FileName = Picture.FileName;
                using (var memoryStream = new MemoryStream())
                {
                    await Picture.CopyToAsync(memoryStream);
                    product.Picture = memoryStream.ToArray();
                }
                    
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            SelectList categories = new SelectList(_context.Categories, "Id", "Title");
            ViewBag.Categories = categories;
            return View(product);
        }
        [Authorize(Roles = "Administrator")]
        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            SelectList categories = new SelectList(_context.Categories, "Id", "Title");
            ViewBag.Categories = categories;
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Cost,FileName,Description,CategoryId")] Product product, IFormFile Picture)
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
                    await _context.SaveChangesAsync();
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
                product.FileName = Picture.FileName;
                using (var memoryStream = new MemoryStream())
                {
                    await Picture.CopyToAsync(memoryStream);
                    product.Picture = memoryStream.ToArray();
                }
                _context.Update(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            SelectList categories = new SelectList(_context.Categories, "Id", "Title");
            ViewBag.Categories = categories;
            return View(product);
        }

        // GET: Products/Picture/5
        public async Task<IActionResult> Picture(int? id, int x, int y)
        {
            if (id == null)
            {
                return NotFound();
            }
            // Construct absolute image path
            //var imagePath = "whatever";
            var product = await _context.Products
               .FirstOrDefaultAsync(m => m.Id == id);
            var im = new WebImage(product.Picture)
           .Resize(x, y, false, true) // Resizing the image to 100x100 px on the fly...
           .Crop(1, 1) // Cropping it to remove 1px border at top and left sides (bug in WebImage)
           .GetBytes();
            if (product == null)
            {
                return NotFound();
            }
            return base.File(im, "image/jpg");
        }

        // GET: Products/Delete/5
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

    }
}
