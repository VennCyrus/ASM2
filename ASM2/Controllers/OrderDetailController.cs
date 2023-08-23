    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FPTBOOK_STORE.Models;
using FPTBOOK_STORE.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
namespace FPTBOOK_STORE.Controllers
{
    public class OrderDetailController : Controller
    {   
        private string Layout ="StoreownerLayout"; 
        private string MainLayout ="_MainLayout"; 
        private readonly FPTBOOK_STOREIdentityDbContext _context;

        public OrderDetailController(FPTBOOK_STOREIdentityDbContext context)
        {
            _context = context;
        }

        // GET: OrderDetail
        [Authorize(Roles = "StoreOwner")]
        public async Task<IActionResult> Index(int id)
        {
            ViewBag.Layout = Layout;
            var FPTBOOK_STOREIdentityDbContext = _context.OrderDetail.Include(o => o.Book).Include(o => o.Order);
            ViewBag.id = id;
            return View(await FPTBOOK_STOREIdentityDbContext.ToListAsync());
        }
        public async Task<IActionResult> Index1(int id)
        {
            ViewBag.Layout = MainLayout;
            var FPTBOOK_STOREIdentityDbContext = _context.OrderDetail.Include(o => o.Book).Include(o => o.Order);
            ViewBag.id = id;
            return View(await FPTBOOK_STOREIdentityDbContext.ToListAsync());
        }

        // GET: OrderDetail/Details/5
        [Authorize(Roles = "StoreOwner")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.OrderDetail == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetail
                .Include(o => o.Book)
                .Include(o => o.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // GET: OrderDetail/Create
        [Authorize(Roles = "StoreOwner")]
        public IActionResult Create()
        {
            ViewData["BookID"] = new SelectList(_context.Book, "Id", "Id");
            ViewData["OrderID"] = new SelectList(_context.Order, "Id", "Id");
            return View();
        }

        // POST: OrderDetail/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Quantity,OrderID,BookID")] OrderDetail orderDetail)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orderDetail);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BookID"] = new SelectList(_context.Book, "Id", "Id", orderDetail.BookID);
            ViewData["OrderID"] = new SelectList(_context.Order, "Id", "Id", orderDetail.OrderID);
            return View(orderDetail);
        }

        // GET: OrderDetail/Edit/5
        [Authorize(Roles = "StoreOwner")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.OrderDetail == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetail.FindAsync(id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            ViewData["BookID"] = new SelectList(_context.Book, "Id", "Id", orderDetail.BookID);
            ViewData["OrderID"] = new SelectList(_context.Order, "Id", "Id", orderDetail.OrderID);
            return View(orderDetail);
        }

        // POST: OrderDetail/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Quantity,OrderID,BookID")] OrderDetail orderDetail)
        {
            if (id != orderDetail.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orderDetail);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderDetailExists(orderDetail.Id))
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
            ViewData["BookID"] = new SelectList(_context.Book, "Id", "Id", orderDetail.BookID);
            ViewData["OrderID"] = new SelectList(_context.Order, "Id", "Id", orderDetail.OrderID);
            return View(orderDetail);
        }

        // GET: OrderDetail/Delete/5
        [Authorize(Roles = "StoreOwner")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.OrderDetail == null)
            {
                return NotFound();
            }

            var orderDetail = await _context.OrderDetail
                .Include(o => o.Book)
                .Include(o => o.Order)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orderDetail == null)
            {
                return NotFound();
            }

            return View(orderDetail);
        }

        // POST: OrderDetail/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.OrderDetail == null)
            {
                return Problem("Entity set 'FPTBOOK_STOREIdentityDbContext.OrderDetail'  is null.");
            }
            var orderDetail = await _context.OrderDetail.FindAsync(id);
            if (orderDetail != null)
            {
                _context.OrderDetail.Remove(orderDetail);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderDetailExists(int id)
        {
          return (_context.OrderDetail?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
