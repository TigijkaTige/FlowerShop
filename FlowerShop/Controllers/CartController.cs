using FlowerShop.Data;
using FlowerShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.Web.Helpers;

namespace FlowerShop.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<User> _userManager;

        //class constructor
        
        public CartController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Authorize]
        //без понятие как мы пользовател передаем
        public async Task<IActionResult> Index()
        {
            var Userid = _userManager.GetUserId(User);
            if (Userid == null)
            {
                return NotFound();
            }
            var myCart = _context.Carts.Where(c => c.User.Id==Userid);
            if (myCart == null)
            {
                return View();
            }
            ViewBag.Products = _context.Products.ToList();
            
            var u = _context.Carts.Where(m => m.UserId == Userid).Select(m => m.Product.Cost * m.CountPr).Sum();
            return View(myCart);
            //return View();
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
           .Resize(x, y, false, true) // Resizing the image to 100x100 px on the fly...
           .Crop(1, 1) // Cropping it to remove 1px border at top and left sides (bug in WebImage)
           .GetBytes();
            if (product == null)
            {
                return NotFound();
            }
            return base.File(im, "image/jpg");

        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int productId, int CountPr)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var d = _context.Carts.Where(m=>m.Product.Id == productId && m.UserId ==userId );
                if (!d.IsNullOrEmpty()) { 
                  var cart1 = d.First();
                    cart1.CountPr +=CountPr;
                    _context.Update(cart1);
                    await _context.SaveChangesAsync();
                    return Ok(); 

                }

                Cart cart = new Cart();
                
                cart.Product = _context.Products.Find(productId);
                cart.CountPr = CountPr;
                cart.UserId = userId;  
                _context.Add(cart);
                
                await _context.SaveChangesAsync();
               return RedirectToAction(nameof(Index));
            }
            //ViewBag.Products = _context.Products;
            return View();
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cart = await _context.Carts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cart == null)
            {
                return NotFound();
            }

            return View(cart);
        }
        private bool CartExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }

}
