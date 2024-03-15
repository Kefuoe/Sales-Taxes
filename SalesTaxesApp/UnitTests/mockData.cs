using SalesTaxesApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesTaxesApp.Services;
using SalesTaxesApp.Data;
using SalesTaxesApp.Pages;
namespace SalesTaxesApp.UnitTests
{
    public class mockData
    {
        public static List<Item> GetMockItems()
        {
            return new List<Item>
            {
                new Item { Name = "Book", Price = 12.49m, Type = ItemType.Book },
                new Item { Name = "Music CD", Price = 14.99m, Type = ItemType.Other },
                new Item { Name = "Chocolate bar", Price = 0.85m, Type = ItemType.Food }
            };
        }
    }
}