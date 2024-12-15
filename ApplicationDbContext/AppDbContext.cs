using Microsoft.EntityFrameworkCore;
using MyGoldenFood.Models;

namespace MyGoldenFood.ApplicationDbContext
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }


        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
