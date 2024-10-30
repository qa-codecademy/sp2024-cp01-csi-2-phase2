using Login__Register_Endpoints.Models;
using Microsoft.EntityFrameworkCore;

namespace Login__Register_Endpoints.Controllers
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=Bookstore;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }
        public DbSet<User> Users { get; set; }
        // dodadi

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
