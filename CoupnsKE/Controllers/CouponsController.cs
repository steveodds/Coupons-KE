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
using Microsoft.AspNetCore.Authorization;

namespace CoupnsKE.Controllers
{
    public class CouponsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<CoupnsKEUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly bool isAuthorized = false;

        public CouponsController(ApplicationDbContext context, UserManager<CoupnsKEUser> userManager, IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            isAuthorized = _userManager.GetUserAsync(_contextAccessor.HttpContext.User).Result.UserRole == Enum.Roles.Administrator;
        }

        // GET: Coupons
        public async Task<IActionResult> Index()
        {
            ViewData["isAuthorised"] = isAuthorized;
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SaveCoupon(Guid couponID)
        {
            var coupon = await _context.Coupon.FindAsync(couponID);
            var user = _userManager.GetUserAsync(_contextAccessor.HttpContext.User).Result.Id;
            var hasSavedCoupons = _context.UserCoupons.Where(x => x.UserID == user);
            if (hasSavedCoupons.Any())
            {
                //var existingSavedCoupons = hasSavedCoupons.FirstOrDefault();
                if (hasSavedCoupons.Where(x => x.CouponID == couponID).Any())
                {
                    return RedirectToAction("Index", "UserCoupons");
                }
                else
                {
                    var newCoupon = new UserCoupons()
                    {
                        CouponID = couponID,
                        UserID = user
                    };
                    _context.UserCoupons.Add(newCoupon);
                }
                //_context.Update(existingSavedCoupons);
            }
            else
            {
                var userCoupon = new UserCoupons
                {
                    CouponID = couponID,
                    UserID = user
                };
                _context.UserCoupons.Add(userCoupon);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "UserCoupons");
        }

        private bool CouponExists(Guid id)
        {
            return _context.Coupon.Any(e => e.CouponID == id);
        }
    }
}
