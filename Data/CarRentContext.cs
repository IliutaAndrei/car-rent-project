using CarRent.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CarRent.Data
{
    public class CarRentContext : IdentityDbContext
    {
        public CarRentContext(DbContextOptions<CarRentContext> options) : base(options)
        {

        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Order> Orders { get; set; }

        //function for model configurations (constraints, relationhips)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurare TPT
            //modelBuilder.Entity<Customer>().ToTable("Customers");
            // modelBuilder.Entity<Admin>().ToTable("Admins");

            // Cheia primară pentru User
            // modelBuilder.Entity<User>().HasKey(u => u.UserId);

            // Relații (restul configurației tale)
            // modelBuilder.Entity<Order>()
            //  .HasOne(o => o.Customer)
            //  .WithMany(c => c.Orders)
            // .HasForeignKey(o => o.CustomerID);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Car)
                .WithMany()
                .HasForeignKey(o => o.CarID);

            //  modelBuilder.Entity<User>()
            //  .HasIndex(u => u.Email)
            //.IsUnique();
        }
    }
}