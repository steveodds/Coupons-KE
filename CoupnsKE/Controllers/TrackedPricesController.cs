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
using CoupnsKE.Services.Web.Interfaces;
using System.Net.Http;
using CoupnsKE.Services.Web.Scraper;
using Microsoft.AspNetCore.Identity;
using CoupnsKE.Areas.Identity.Data;
using Microsoft.AspNetCore.Http;

namespace CoupnsKE.Controllers
{
    [Authorize]
    public class TrackedPricesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IScraper _scraper;
        private readonly UserManager<CoupnsKEUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public TrackedPricesController(ApplicationDbContext context,
            IScraper scraper, UserManager<CoupnsKEUser> userManager,
            IHttpContextAccessor contextAccessor)
        {
            _context = context;
            _scraper = scraper;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }

        // GET: TrackedPrices
        public async Task<IActionResult> Index()
        {
            var trackers = await _context.TrackedPrice.ToListAsync();
            List<Product> products = new List<Product>();
            foreach (var tracker in trackers)
            {
                products.Add(_context.Product.FindAsync(tracker.ProductID).Result);
            }

            var trackedItems = new TrackedItems();
            trackedItems.products = products;
            trackedItems.trackers = trackers;

            return View(trackedItems);
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
        public async Task<IActionResult> Create(string searchString)
        {
            if (searchString == null)
                return View();

            string url = GenerateSearchUrl(searchString, "Jumia");

            string htmlDocument;
            using (var client = new HttpClient())
            {
                using (var response = client.GetAsync(url).Result)
                {
                    using (var content = response.Content)
                    {
                        htmlDocument = content.ReadAsStringAsync().Result;
                    }
                }
            }
            var productList = await _scraper.GetMultipleProductsAsync(url, htmlDocument, null, null);

            return View(productList);
        }

        private string GenerateSearchUrl(string searchString, string store)
        {
            searchString = searchString.Trim().ToLower();
            store = store.Trim().ToLower();
            searchString = searchString.Replace(" ", "+");
            switch (store)
            {
                case "jumia":
                    return $@"https://www.jumia.co.ke/catalog/?q={searchString}&sort=highest-price&shipped_from=country_local";
                default:
                    return null;
            }
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

        //GET: Alternative section for using existing links
        public IActionResult ProductLink()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ProductLink(string? url)
        {
            if (url == null)
                return View();

            string htmlDocument;
            using (var client = new HttpClient())
            {
                using (var response = client.GetAsync(url).Result)
                {
                    using (var content = response.Content)
                    {
                        htmlDocument = content.ReadAsStringAsync().Result;
                    }
                }
            }
            var product = await _scraper.GetSingleProductAsync(url, htmlDocument);
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Index(Guid productID, decimal desiredPrice)
        {
            var tracker = new TrackedPrice();
            var product = await _context.Product.FindAsync(productID);
            var existingTracker = _context.TrackedPrice.Where(x => x.ProductID == product.ProductID);
            if (existingTracker.Any())
            {
                var updateTracker = existingTracker.FirstOrDefault();
                updateTracker.DesiredPrice = desiredPrice;
                if (updateTracker.LowestPrice > product.Price)
                    updateTracker.LowestPrice = product.Price;
                _context.TrackedPrice.Update(updateTracker);
            }
            else
            {
                tracker.DesiredPrice = desiredPrice;
                tracker.LowestPrice = product.Price;
                tracker.ProductID = product.ProductID;
                tracker.StoreWithLowestPrice = product.StoreName;
                tracker.TrackedPriceID = new Guid();
                tracker.UserID = _userManager.GetUserAsync(_contextAccessor.HttpContext.User).Result.Id;
                _context.TrackedPrice.Add(tracker);
            }


            await _context.SaveChangesAsync();
            //var trackers = new List<TrackedPrice>();
            //trackers.Add(tracker);

            //return View();
            return RedirectToAction("Index");
        }


        private bool TrackedPriceExists(Guid id)
        {
            return _context.TrackedPrice.Any(e => e.TrackedPriceID == id);
        }

        public class TrackedItems
        {
            public List<Product> products { get; set; }
            public List<TrackedPrice> trackers { get; set; }
        }
    }
}
