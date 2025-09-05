using Hotel.Models;
using Microsoft.EntityFrameworkCore;

namespace ProjectName.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<UnitImage> UnitImages { get; set; }

        public DbSet<Unit> Units { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Expense> Expenses { get; set; }

    }
}
