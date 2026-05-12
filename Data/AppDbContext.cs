using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using GrampanchayatSystem.Models;

namespace GrampanchayatSystem.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Citizen> Citizens { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<TaxRecord> TaxRecords { get; set; }
    }
}