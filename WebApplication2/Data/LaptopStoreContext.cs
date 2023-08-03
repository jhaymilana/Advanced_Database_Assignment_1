using WebApplication2.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApplication2.Data
{
    public class LaptopStoreContext : DbContext
    {
        public LaptopStoreContext(DbContextOptions
            options) : base(options) { }
        public DbSet<Brand> Brands { get; set; } = null!;
        public DbSet<Laptop> Laptops { get; set; } = null!;
        public DbSet<Store> Stores { get; set; } = null!;
    }
}
