using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoupnsKE.Data;
using CouponsKE.Models;

namespace CoupnsKE.Controllers
{
    public class DealsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DealsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Deals
        public async Task<IActionResult> Index()
        {
            return View(await _context.Deals.ToListAsync());
        }

        // GET: Deals/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deals = await _context.Deals
                .FirstOrDefaultAsync(m => m.DealsID == id);
            if (deals == null)
            {
                return NotFound();
            }

            return View(deals);
        }

        // GET: Deals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Deals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DealsID,DealName,DealDescription,DealRating,Store,OldPrice,NewPrice")] Deals deals)
        {
            if (ModelState.IsValid)
            {
                deals.DealsID = Guid.NewGuid();
                _context.Add(deals);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(deals);
        }

        // GET: Deals/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deals = await _context.Deals.FindAsync(id);
            if (deals == null)
            {
                return NotFound();
            }
            return View(deals);
        }

        // POST: Deals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("DealsID,DealName,DealDescription,DealRating,Store,OldPrice,NewPrice")] Deals deals)
        {
            if (id != deals.DealsID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(deals);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DealsExists(deals.DealsID))
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
            return View(deals);
        }

        // GET: Deals/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var deals = await _context.Deals
                .FirstOrDefaultAsync(m => m.DealsID == id);
            if (deals == null)
            {
                return NotFound();
            }

            return View(deals);
        }

        // POST: Deals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var deals = await _context.Deals.FindAsync(id);
            _context.Deals.Remove(deals);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DealsExists(Guid id)
        {
            return _context.Deals.Any(e => e.DealsID == id);
        }
    }
}
