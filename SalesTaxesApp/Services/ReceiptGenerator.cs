using SalesTaxesApp.Entities;
using SalesTaxesApp.Interfaces;

namespace SalesTaxesApp.Services
{
    public class ReceiptGenerator
    {
        private readonly IDataContext _context;

        public ReceiptGenerator(IDataContext context)
        {
            _context = context;
        }

        public async Task<Receipt> GenerateReceiptAsync(string inputBasket)
        {
            try {
                if (_context == null)
                {
                    throw new InvalidOperationException("_context is null. Make sure it is properly initialized.");
                }

                // Parse inputBasket and extract item details
                var items = ParseInputBasket(inputBasket);

                if (items == null || items.Count == 0)
                    return null;

                // Calculate total price and total tax
                decimal totalPrice = 0;
                decimal totalTax = 0;
                foreach (var item in items)
                {
                    decimal tax = CalculateTax(item);
                    item.Price += tax; // Update item price with tax included
                    totalPrice += item.Price;
                    totalTax += tax;
                }

                // Save items to database
                var basket = new Basket { Items = items };
                await _context.Baskets.AddAsync(basket);
                await _context.SaveChangesAsync();

                // Generate receipt
                var receipt = new Receipt
                {
                    Basket = basket,
                    TotalPrice = totalPrice,
                    TotalTax = totalTax
                };

                await _context.Receipts.AddAsync(receipt);
                await _context.SaveChangesAsync();

                return receipt;
            }
            catch (Exception ex)
            {
                // Log the exception for debugging
                Console.WriteLine($"Exception occurred: {ex.Message}");
            }  
            
            return null;
        }

        public List<Item> ParseInputBasket(string inputBasket)
        {
            List<Item> items = new List<Item>();

            // Split inputBasket by lines
            string[] lines = inputBasket.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

            foreach (string line in lines)
            {
                string[] parts = line.Split(new[] { " at " }, StringSplitOptions.None);

                if (parts.Length != 2)
                {
                    // Invalid format, skip this line
                    continue;
                }

                string itemInfo = parts[0];
                decimal price;

                // Extract price
                if (!decimal.TryParse(parts[1], out price))
                {
                    // Invalid price format, skip this line
                    continue;
                }

                string[] itemParts = itemInfo.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                // Extract quantity and name
                int quantity;
                if (itemParts.Length < 2 || !int.TryParse(itemParts[0], out quantity))
                {
                    // Invalid quantity or item name, skip this line
                    continue;
                }

                string itemName = string.Join(" ", itemParts.Skip(1));

                // Determine if the item is imported
                bool isImported = itemName.Contains("imported", StringComparison.OrdinalIgnoreCase);

                // Determine the item type
                ItemType itemType;
                if (itemName.Contains("book", StringComparison.OrdinalIgnoreCase))
                {
                    itemType = ItemType.Book;
                }
                else if (itemName.Contains("chocolate", StringComparison.OrdinalIgnoreCase))
                {
                    itemType = ItemType.Food;
                }
                else if (itemName.Contains("pill", StringComparison.OrdinalIgnoreCase) || itemName.Contains("paracetamol", StringComparison.OrdinalIgnoreCase))
                {
                    itemType = ItemType.Medical;
                }
                else
                {
                    itemType = ItemType.Other;
                }

                // Create Item object and add it to the list
                Item item = new Item
                {
                    Name = itemName,
                    Price = price,
                    IsImported = isImported,
                    Type = itemType
                };

                // Add the item according to the specified quantity
                for (int i = 0; i < quantity; i++)
                {
                    items.Add(item);
                }
            }

            return items;
        }

        private decimal CalculateTax(Item item)
        {
            decimal taxRate = GetTaxRate(item);
            decimal tax = Math.Ceiling(item.Price * taxRate / 0.05m) * 0.05m;
            return tax;
        }

        private decimal GetTaxRate(Item item)
        {
            if (item.IsImported && item.Type == ItemType.Other)
            {
                return 0.15m; // 10% basic tax + 5% import duty
            }
            else if (item.IsImported && item.Type != ItemType.Other)
            {
                return 0.05m; // 10% basic tax + 5% import duty
            }
            else
            {
                switch (item.Type)
                {
                    case ItemType.Book:
                    case ItemType.Food:
                    case ItemType.Medical:
                        return 0m; // Exempt from basic tax
                    default:
                        return 0.10m; // 10% basic tax
                }
            }
        }
    }
}