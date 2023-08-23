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
    public class PublisherController : Controller
    {
        private readonly FPTBOOK_STOREIdentityDbContext _context;
        private string Layout = "StoreownerLayout";
        public PublisherController(FPTBOOK_STOREIdentityDbContext context)
        {
            _context = context;
        }

        // GET: Publisher
        [Authorize(Roles = "StoreOwner")]
        public async Task<IActionResult> Index()
        {
            ViewBag.Layout = Layout;
            return _context.Publisher != null ?
                        View(await _context.Publisher.ToListAsync()) :
                        Problem("Entity set 'FPTBOOK_STOREIdentityDbContext.Publisher'  is null.");
        }

        // GET: Publisher/Details/5
        [Authorize(Roles = "StoreOwner")]
        public async Task<IActionResult> Details(int? id)
        {
            ViewBag.Layout = Layout;
            if (id == null || _context.Publisher == null)
            {
                return NotFound();
            }

            var publisher = await _context.Publisher
                .FirstOrDefaultAsync(m => m.Id == id);
            if (publisher == null)
            {
                return NotFound();
            }

            return View(publisher);
        }

        // GET: Publisher/Create
        [Authorize(Roles = "StoreOwner")]
        public IActionResult Create()
        {
            ViewBag.Layout = Layout;
            return View();
        }

        // POST: Publisher/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "StoreOwner")]
        public async Task<IActionResult> Create([Bind("Id,Name")] Publisher publisher)
        {
            ViewBag.Layout = Layout;
            if (ModelState.IsValid)
            {
                _context.Add(publisher);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(publisher);
        }

        // GET: Publisher/Edit/5
        [Authorize(Roles = "StoreOwner")]
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.Layout = Layout;
            if (id == null || _context.Publisher == null)
            {
                return NotFound();
            }

            var publisher = await _context.Publisher.FindAsync(id);
            if (publisher == null)
            {
                return NotFound();
            }
            return View(publisher);
        }

        // POST: Publisher/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "StoreOwner")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Publisher publisher)
        {
            ViewBag.Layout = Layout;
            if (id != publisher.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(publisher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PublisherExists(publisher.Id))
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
            return View(publisher);
        }

        // GET: Publisher/Delete/5
        [Authorize(Roles = "StoreOwner")]
        public async Task<IActionResult> Delete(int? id)
        {
            ViewBag.Layout = Layout;
            if (id == null || _context.Publisher == null)
            {
                return NotFound();
            }

            var publisher = await _context.Publisher
                .FirstOrDefaultAsync(m => m.Id == id);
            if (publisher == null)
            {
                return NotFound();
            }

            return View(publisher);
        }

        // POST: Publisher/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewBag.Layout = Layout;
            if (_context.Publisher == null)
            {
                return Problem("Entity set 'FPTBOOK_STOREIdentityDbContext.Publisher'  is null.");
            }
            var publisher = await _context.Publisher.FindAsync(id);
            if (publisher != null)
            {
                _context.Publisher.Remove(publisher);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PublisherExists(int id)
        {
            ViewBag.Layout = Layout;
            return (_context.Publisher?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
