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
    public class UserCouponsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UserCouponsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: UserCoupons
        public async Task<IActionResult> Index()
        {
            var userCoupons = await _context.UserCoupons.ToListAsync();
            List<Coupon> coupons = new List<Coupon>();
            foreach (var saved in userCoupons)
            {
                var coupon = await _context.Coupon.FindAsync(saved.CouponID);
                coupons.Add(coupon);
            }
            ViewData["coupons"] = coupons;
            return View(userCoupons);
        }

        // GET: UserCoupons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCoupons = await _context.UserCoupons
                .FirstOrDefaultAsync(m => m.ID == id);
            if (userCoupons == null)
            {
                return NotFound();
            }

            return View(userCoupons);
        }

        // GET: UserCoupons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UserCoupons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,UserID")] UserCoupons userCoupons)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userCoupons);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userCoupons);
        }

        // GET: UserCoupons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCoupons = await _context.UserCoupons.FindAsync(id);
            if (userCoupons == null)
            {
                return NotFound();
            }
            return View(userCoupons);
        }

        // POST: UserCoupons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,UserID")] UserCoupons userCoupons)
        {
            if (id != userCoupons.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userCoupons);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserCouponsExists(userCoupons.ID))
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
            return View(userCoupons);
        }

        // GET: UserCoupons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userCoupons = await _context.UserCoupons
                .FirstOrDefaultAsync(m => m.ID == id);
            if (userCoupons == null)
            {
                return NotFound();
            }

            return View(userCoupons);
        }

        // POST: UserCoupons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userCoupons = await _context.UserCoupons.FindAsync(id);
            _context.UserCoupons.Remove(userCoupons);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserCouponsExists(int id)
        {
            return _context.UserCoupons.Any(e => e.ID == id);
        }
    }
}
