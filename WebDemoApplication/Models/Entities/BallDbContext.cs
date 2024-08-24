using Microsoft.EntityFrameworkCore;

namespace WebDemoApplication.Models.Entities
{
    public class BallDbContext : DbContext
    {
        public BallDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Ball> Balls { get; set; }
        public DbSet<CartItem> CartItems{ get; set; }

    }
}
