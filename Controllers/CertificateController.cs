using Microsoft.AspNetCore.Mvc;
using GrampanchayatSystem.Data;
using GrampanchayatSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace GrampanchayatSystem.Controllers
{
    [Authorize]
    public class CertificateController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CertificateController(AppDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var dataQuery = _context.Certificates.AsQueryable();

            if (!User.IsInRole("Admin"))
            {
                var userId = _userManager.GetUserId(User);
                dataQuery = dataQuery.Where(x => x.RequestedByUserId == userId);
            }

            var data = await dataQuery
                .OrderByDescending(x => x.RequestedAt)
                .ToListAsync();

            return View(data);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Certificate c)
        {
            if (!ModelState.IsValid)
            {
                return View(c);
            }

            c.RequestedByUserId = _userManager.GetUserId(User) ?? string.Empty;
            c.RequestedAt = DateTime.UtcNow;
            c.Status = "Pending";
            c.PaymentStatus = "Unpaid";

            _context.Certificates.Add(c);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var certificate = await _context.Certificates.FindAsync(id);
            if (certificate is null)
            {
                return NotFound();
            }

            certificate.Status = "Approved";
            certificate.CertificateNumber ??= $"CERT-{DateTime.UtcNow:yyyyMMdd}-{certificate.Id:D4}";
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reject(int id)
        {
            var certificate = await _context.Certificates.FindAsync(id);
            if (certificate is null)
            {
                return NotFound();
            }

            certificate.Status = "Rejected";
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pay(int id)
        {
            var certificate = await _context.Certificates.FindAsync(id);
            if (certificate is null)
            {
                return NotFound();
            }

            // var userId = _userManager.GetUserId(User);
            // if (!User.IsInRole("Admin") && certificate.RequestedByUserId != userId)
            // {
            //     return Forbid();
            // }

            certificate.PaymentStatus = "Paid";
            certificate.PaymentReference = $"PAY-{Guid.NewGuid():N}"[..12].ToUpperInvariant();
            await _context.SaveChangesAsync();

            TempData["Message"] = "Payment completed successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> DownloadPdf(int id)
        {
            var certificate = await _context.Certificates.FindAsync(id);
            if (certificate is null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (!User.IsInRole("Admin") && certificate.RequestedByUserId != userId)
            {
                return Forbid();
            }

            if (certificate.Status != "Approved" || certificate.PaymentStatus != "Paid")
            {
                TempData["Message"] = "Certificate must be approved and paid before download.";
                return RedirectToAction(nameof(Index));
            }

            QuestPDF.Settings.License = LicenseType.Community;
            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);
                    page.Size(PageSizes.A4);
                    page.DefaultTextStyle(x => x.FontSize(12));

                    page.Content().Column(col =>
                    {
                        col.Spacing(12);
                        col.Item().AlignCenter().Text("Gram Panchayat Certificate").FontSize(22).Bold();
                        col.Item().Text($"Certificate No: {certificate.CertificateNumber}");
                        col.Item().Text($"Citizen Name: {certificate.CitizenName}");
                        col.Item().Text($"Certificate Type: {certificate.Type}");
                        col.Item().Text($"Details: {certificate.Details}");
                        col.Item().Text($"Payment Ref: {certificate.PaymentReference}");
                        col.Item().Text($"Issued On: {DateTime.Now:dd MMM yyyy}");
                    });
                });
            }).GeneratePdf();

            var filename = $"certificate-{certificate.Id}.pdf";
            return File(pdfBytes, "application/pdf", filename);
        }
    }
}