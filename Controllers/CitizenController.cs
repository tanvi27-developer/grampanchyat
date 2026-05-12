using Microsoft.AspNetCore.Mvc;
using GrampanchayatSystem.Data;
using GrampanchayatSystem.Models;

namespace GrampanchayatSystem.Controllers
{
    public class CitizenController : Controller
    {
        private readonly AppDbContext _context;

        public CitizenController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var data = _context.Citizens.ToList();
            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Citizen c)
        {
            _context.Citizens.Add(c);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Edit
        public IActionResult Edit(int id)
        {
            var data = _context.Citizens.Find(id);
            return View(data);
        }

        // POST: Edit
        [HttpPost]
        public IActionResult Edit(Citizen c)
        {
            _context.Citizens.Update(c);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var citizen = _context.Citizens.Find(id);
            if (citizen != null)
            {
                _context.Citizens.Remove(citizen);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}