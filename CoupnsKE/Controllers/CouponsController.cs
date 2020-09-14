using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoupnsKE.Data;
using CouponsKE.Models;
using Microsoft.AspNetCore.Identity;
using CoupnsKE.Areas.Identity.Data;
using Microsoft.AspNetCore.Http;

namespace CoupnsKE.Controllers
{
    public class CouponsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<CoupnsKEUser> _userManager;
        private readonly bool isAuthorized = false;

        public CouponsController(ApplicationDbContext context, UserManager<CoupnsKEUser> userManager, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _userManager = userManager;
            isAuthorized = _userManager.GetUserAsync(contextAccessor.HttpContext.User).Result.UserRole == Enum.Roles.Administrator;
        }

        // GET: Coupons
        public async Task<IActionResult> Index()
        {
            return View(await _context.Coupon.ToListAsync());
        }

        // GET: Coupons/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (!isAuthorized)
                return NotFound();

            if (id == null || !isAuthorized)
            {
                return NotFound();
            }

            var coupon = await _context.Coupon
                .FirstOrDefaultAsync(m => m.CouponID == id);
            if (coupon == null)
            {
                return NotFound();
            }

            return View(coupon);
        }

        // GET: Coupons/Create
        public IActionResult Create()
        {
            if (!isAuthorized)
                return NotFound();
            return View();
        }

        // POST: Coupons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CouponID,Store,CouponCategory,CouponCode,Description,ExpiryDate,Restrictions,CouponUrl")] Coupon coupon)
        {
            if (!isAuthorized)
                return NotFound();

            if (ModelState.IsValid)
            {
                coupon.CouponID = Guid.NewGuid();
                _context.Add(coupon);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(coupon);
        }

        // GET: Coupons/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null || !isAuthorized)
            {
                return NotFound();
            }

            var coupon = await _context.Coupon.FindAsync(id);
            if (coupon == null)
            {
                return NotFound();
            }
            return View(coupon);
        }

        // POST: Coupons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("CouponID,Store,CouponCategory,CouponCode,Description,ExpiryDate,Restrictions,CouponUrl,StoreSeller")] Coupon coupon)
        {
            if (!isAuthorized)
                return NotFound();

            if (id != coupon.CouponID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coupon);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CouponExists(coupon.CouponID))
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
            return View(coupon);
        }

        // GET: Coupons/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null || !isAuthorized)
            {
                return NotFound();
            }

            var coupon = await _context.Coupon
                .FirstOrDefaultAsync(m => m.CouponID == id);
            if (coupon == null)
            {
                return NotFound();
            }

            return View(coupon);
        }

        // POST: Coupons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            if (!isAuthorized)
                return NotFound();
            var coupon = await _context.Coupon.FindAsync(id);
            _context.Coupon.Remove(coupon);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool CouponExists(Guid id)
        {
            return _context.Coupon.Any(e => e.CouponID == id);
        }
    }
}
