using FlowerShop.Data;
using FlowerShop.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using static NuGet.Packaging.PackagingConstants;

namespace FlowerShop.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private UserManager<User> _userManager;
        public OrderController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            
            return View();
        }
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DateTime DeliverDate, string Address )
        {
            var userId = _userManager.GetUserId(User);
            
            if (ModelState.IsValid)
            {
                Orders orders = new Orders();
                orders.OrderDate = DateTime.Now;
                orders.Address = Address;
                orders.DeliverDate = DeliverDate;   
                orders.UserId = userId;
                orders.Delivered = false;
                var u = _context.Carts.Where(m => m.UserId == userId).Select(m => m.Product.Cost * m.CountPr).Sum();
                orders.Summa = u;
                _context.Add(orders);

                
                await _context.SaveChangesAsync();
                var d = _context.Carts.Where(m => m.UserId == userId);
               
                
                foreach (var cartItem in d)
                {
                    
                  var item=  new OrderItem();
                    item.ProductId = cartItem.ProductId;
                    item.CountPr = cartItem.CountPr;
                    item.OrdersId = orders.Id;
                    _context.Add(item);
                    _context.Remove(cartItem);
                }
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), orders.Id);
                
            }
            //Еще где-то тут мы перекопируем корзину в ordersItem
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Details()
        {

            var Userid = _userManager.GetUserId(User);
            if (Userid == null)
            {
                return NotFound();
            }
            var myCart = _context.Orders.Where(c => c.User.Id == Userid);
            if (myCart == null)
            {
                return View();
            }
            ViewBag.Products = _context.Items.ToList();
            return View();
        }
       /* public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categories = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categories == null)
            {
                return NotFound();
            }

            return View(categories);
        }*/
    }
}
