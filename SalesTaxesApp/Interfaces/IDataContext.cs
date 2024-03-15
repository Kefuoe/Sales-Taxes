using Microsoft.EntityFrameworkCore;
using SalesTaxesApp.Entities;

namespace SalesTaxesApp.Interfaces
{
    public interface IDataContext
    {
        DbSet<Item> Items { get; }
        DbSet<Receipt> Receipts { get; }
        DbSet<Basket> Baskets { get; }
        Task<int> SaveChangesAsync();
    }
}
