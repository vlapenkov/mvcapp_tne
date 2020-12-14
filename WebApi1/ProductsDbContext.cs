using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi1
{
    public class ProductsDbContext :DbContext
    {
        public ProductsDbContext(DbContextOptions<ProductsDbContext> options):base(options)
        {

        }


        public DbSet<Product> Products { get; set; }
       

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //    => optionsBuilder.UseNpgsql("Host=localhost;Database=products_db;Username=TNE_USER;Password=123123");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(
         new Product{ Id = 1, Name = "First product" },
         new Product { Id = 2, Name = "Second product" },
            new Product { Id=3,  Name = "Third product" },

            new Product { Id = 4, Name = "Forth product" });
        }
    }
}
