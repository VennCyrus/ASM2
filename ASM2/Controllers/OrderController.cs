using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FPTBOOK_STORE.Models;
using FPTBOOK_STORE.Utils;
using FPTBOOK_STORE.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
namespace FPTBOOK_STORE.Controllers
{
    public class OrderController : Controller
    {
        private string Layout = "StoreownerLayout";
        private string MainLayout = "_MainLayout";
        private readonly UserManager<FPTBOOKUser> _userManager;
        private readonly FPTBOOK_STOREIdentityDbContext _context;
        private readonly IWebHostEnvironment hostEnvironment;

        public OrderController(FPTBOOK_STOREIdentityDbContext context, UserManager<FPTBOOKUser> userManager, IWebHostEnvironment environment)
        {
            _context = context;
            _userManager = userManager;
            hostEnvironment = environment;
        }
        [Authorize(Roles = "StoreOwner")]
        public async Task<IActionResult> Index()
        {
            ViewBag.Layout = Layout;
            var FPTBOOK_STOREIdentityDbContext = _context.Order.Include(m => m.FPTBOOKUser);
            return View(await FPTBOOK_STOREIdentityDbContext.ToListAsync());
        }
        public async Task<IActionResult> Index1()
        {
            ViewBag.Layout = MainLayout;
            var userID = _userManager.GetUserId(HttpContext.User);
            FPTBOOKUser user = _userManager.FindByIdAsync(userID).Result;
            ViewBag.id = user.Id;
            var FPTBOOK_STOREIdentityDbContext = _context.Order.Include(m => m.FPTBOOKUser);
            return View(await FPTBOOK_STOREIdentityDbContext.ToListAsync());
        }
        public IActionResult PlaceOrder(decimal total)
        {
            ShoppingCart cart = (ShoppingCart)HttpContext.Session.GetObject<ShoppingCart>("cart");
            Order myOrder = new Order();
            myOrder.OrderDate = DateTime.Now;
            myOrder.Status = 0;
            var userID = _userManager.GetUserId(HttpContext.User);
            FPTBOOKUser user = _userManager.FindByIdAsync(userID).Result;
            // if(user == null){
            //     return View("Book");
            // }
            myOrder.FPTBOOKUserId = user.Id;

            _context.Order.Add(myOrder);
            _context.SaveChanges();//this generates the Id for Order

            foreach (var item in cart.Items)
            {
                OrderDetail myOrderItem = new OrderDetail();
                myOrderItem.BookID = item.Id;
                myOrderItem.Quantity = item.Quantity;
                myOrderItem.OrderID = myOrder.Id;//id of saved order above

                _context.OrderDetail.Add(myOrderItem);
            }
            _context.SaveChanges();
            //empty shopping cart
            cart = new ShoppingCart();
            HttpContext.Session.SetObject("cart", cart);
            return RedirectToAction("BookHome", "Book");
        }
        [Authorize(Roles = "StoreOwner")]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Layout = Layout;
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var order = await _context.Order.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "StoreOwner")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OrderDate,Status,FPTBOOKUserId")] Order order)
        {
            
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }
        private bool OrderExists(int id)
        {
            return (_context.Order?.Any(e => e.Id == id)).GetValueOrDefault();

        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Order == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.Order
                .Include(o => o.FPTBOOKUser)
                .Include(o => o.OrderDetails)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Order == null)
            {
                return Problem("Entity set 'FPTBOOK_STOREIdentityDbContext.OrderDetail'  is null.");
            }
            var order = await _context.Order.FindAsync(id);
            if (order != null)
            {
                _context.Order.Remove(order);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction("BookHome","Book");
        }
    }
}