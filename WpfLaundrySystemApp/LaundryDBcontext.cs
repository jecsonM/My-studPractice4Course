using Microsoft.EntityFrameworkCore;

namespace DBContext.LaundryDbContext
{
    public class LaundryDbContext : DbContext
    {
        //public DbSet<User> Users { get; set; }
        public LaundryDbContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=laundryStudPractice4Course;Username=postgres;Password=POSTGREmoiseiev");
        }
    }
}