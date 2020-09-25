using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CoupnsKE.Models;
using Microsoft.AspNetCore.Http;
using CoupnsKE.Data;
using Microsoft.AspNetCore.Identity;
using CoupnsKE.Areas.Identity.Data;

namespace CoupnsKE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly UserManager<CoupnsKEUser> _userManager;
        private readonly SignInManager<CoupnsKEUser> _signInManager;
        public bool IsLoggedIn { get; set; }

        public HomeController(ILogger<HomeController> logger, SignInManager<CoupnsKEUser> signInManager, 
            UserManager<CoupnsKEUser> userManager, IHttpContextAccessor contextAccessor, ApplicationDbContext context)
        {
            _logger = logger;
            //var role = _userManager.GetUserAsync(_contextAccessor.HttpContext.User).Result.UserRole.ToString().ToLower();
            //if (role == "user" || role.Contains("admin"))
            //    IsLoggedIn = true;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Index()
        {
            IsLoggedIn = _signInManager.IsSignedIn(User);
            ViewData["status"] = IsLoggedIn;
            if (IsLoggedIn)
            {
                var userID = _userManager.GetUserAsync(_contextAccessor.HttpContext.User).Result.Id;
                var coupons = _context.UserCoupons.Where(x => x.UserID == userID).Count();
                var trackers = _context.TrackedPrice.Where(x => x.UserID == userID).Count();
                ViewData["coupons"] = coupons;
                ViewData["trackers"] = trackers;
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
