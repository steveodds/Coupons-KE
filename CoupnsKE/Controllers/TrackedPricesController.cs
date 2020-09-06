using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoupnsKE.Data;
using CouponsKE.Models;
using Microsoft.AspNetCore.Authorization;

namespace CoupnsKE.Controllers
{
    [Authorize]
    public class TrackedPricesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrackedPricesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TrackedPrices
        public async Task<IActionResult> Index()
        {
            return View(await _context.TrackedPrice.ToListAsync());
        }

        // GET: TrackedPrices/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trackedPrice = await _context.TrackedPrice
                .FirstOrDefaultAsync(m => m.TrackedPriceID == id);
            if (trackedPrice == null)
            {
                return NotFound();
            }

            return View(trackedPrice);
        }

        // GET: TrackedPrices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TrackedPrices/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TrackedPriceID,UserID,ProductID,DesiredPrice,LowestPrice,StoreWithLowestPrice")] TrackedPrice trackedPrice)
        {
            if (ModelState.IsValid)
            {
                trackedPrice.TrackedPriceID = Guid.NewGuid();
                _context.Add(trackedPrice);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(trackedPrice);
        }

        // GET: TrackedPrices/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trackedPrice = await _context.TrackedPrice.FindAsync(id);
            if (trackedPrice == null)
            {
                return NotFound();
            }
            return View(trackedPrice);
        }

        // POST: TrackedPrices/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("TrackedPriceID,UserID,ProductID,DesiredPrice,LowestPrice,StoreWithLowestPrice")] TrackedPrice trackedPrice)
        {
            if (id != trackedPrice.TrackedPriceID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trackedPrice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrackedPriceExists(trackedPrice.TrackedPriceID))
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
            return View(trackedPrice);
        }

        // GET: TrackedPrices/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trackedPrice = await _context.TrackedPrice
                .FirstOrDefaultAsync(m => m.TrackedPriceID == id);
            if (trackedPrice == null)
            {
                return NotFound();
            }

            return View(trackedPrice);
        }

        // POST: TrackedPrices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var trackedPrice = await _context.TrackedPrice.FindAsync(id);
            _context.TrackedPrice.Remove(trackedPrice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrackedPriceExists(Guid id)
        {
            return _context.TrackedPrice.Any(e => e.TrackedPriceID == id);
        }
    }
}
