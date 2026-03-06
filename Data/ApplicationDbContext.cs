using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopManagementSystem.Models;

namespace ShopManagementSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser,IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductLifting> ProductLiftings { get; set; }
        public DbSet<Sales> Sales { get; set; }
        public DbSet<SalesDetail> SalesDetails { get; set; }
        public DbSet<ProductLiftingHistory> ProductLiftingHistories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder); // ⭐ MUST for Identity

            modelBuilder.Entity<Sales>()
                .Property(x => x.SalesDate)
                .HasDefaultValueSql("GETDATE()");

            //Delete Behavior Control
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProductLiftingHistory>()
                .HasOne(h => h.ProductLifting)
                .WithMany(p => p.ProductLiftingHistories)
                .HasForeignKey(h => h.ProductLiftingId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductLiftingHistory>()
                .HasOne(h => h.Product)
                .WithMany(p => p.ProductLiftingHistories)
                .HasForeignKey(h => h.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
