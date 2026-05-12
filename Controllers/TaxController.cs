using GrampanchayatSystem.Data;
using GrampanchayatSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace GrampanchayatSystem.Controllers
{
    public class TaxController : Controller
    {
        private readonly AppDbContext _context;

        public TaxController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var data = _context.TaxRecords.OrderBy(x => x.DueDate).ToList();
            return View(data);
        }

        public IActionResult Create()
        {
            return View(new TaxRecord { DueDate = DateTime.Today });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TaxRecord taxRecord)
        {
            if (!ModelState.IsValid)
            {
                return View(taxRecord);
            }

            _context.TaxRecords.Add(taxRecord);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
