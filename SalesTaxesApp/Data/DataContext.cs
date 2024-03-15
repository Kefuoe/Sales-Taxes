
using Microsoft.EntityFrameworkCore;
using SalesTaxesApp.Entities;
using SalesTaxesApp.Interfaces;

namespace SalesTaxesApp.Data
{
    public class DataContext : DbContext,IDataContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<Item> Items { get; set; }
        public DbSet<Basket> Baskets { get; set; }
        public DbSet<Receipt> Receipts { get; set; }

        public async Task<int> SaveChangesAsync()
        {
            return await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure entities
            modelBuilder.Entity<Item>().ToTable("Items");
            modelBuilder.Entity<Basket>().ToTable("Baskets");
            modelBuilder.Entity<Receipt>().ToTable("Receipts");
        }
    }
}