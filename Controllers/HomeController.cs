using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GrampanchayatSystem.Models;
using GrampanchayatSystem.Data;
using Microsoft.AspNetCore.Identity;

namespace GrampanchayatSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Citizen c)
        {
            _context.Citizens.Add(c);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Dashboard()
        {
            ViewBag.TotalCitizens = _context.Citizens.Count();
            ViewBag.TotalUsers = _userManager.Users.Count();

            return View();
        }

        public IActionResult History()
        {
            return View();
        }

        [HttpGet("Home/Gallery")]
        [HttpGet("Home/Gallary")]
        public IActionResult Gallery()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}