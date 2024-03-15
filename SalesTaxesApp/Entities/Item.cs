using System;

namespace SalesTaxesApp.Entities
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool IsImported { get; set; }
        public ItemType Type { get; set; }
    }
    public enum ItemType
    {
        Book,
        Food,
        Medical,
        Other
    }

}