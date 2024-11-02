using Microsoft.EntityFrameworkCore;
using TestWebStagiaire2024.Models.Entities;

namespace TestWebStagiaire2024.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Item> Items { get; set; }
    }
}
