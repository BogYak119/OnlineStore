using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.DataAccess.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<LocalUser> LocalUsers { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Subcategory> Subcategories { get; set; }
        public DbSet<Manufacturer> Manufacturers { get; set; }
        public DbSet<Product> Products { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            // Database.EnsureCreated();   
        }
    }
}
