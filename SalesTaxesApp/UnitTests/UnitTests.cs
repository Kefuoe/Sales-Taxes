using SalesTaxesApp.Data;
using SalesTaxesApp.Entities;
using SalesTaxesApp.Services;
using SalesTaxesApp.Interfaces;
using Xunit;
using Moq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SalesTaxesApp.UnitTests
{
    public class UnitTests
    {
        [Fact]
        public async Task GenerateReceiptAsync_ReturnsReceiptWithCorrectTotalPriceAndTotalTax()
        {
            // Arrange
            var mockDataContext = new Mock<IDataContext>();
            var mockDbSetReceipt = new Mock<DbSet<Receipt>>();


            mockDataContext.Setup(m => m.Baskets.AddAsync(It.IsAny<Basket>(), default))
                .ReturnsAsync((EntityEntry<Basket>)null);
            mockDataContext.Setup(m => m.Receipts).Returns(mockDbSetReceipt.Object);

            var receiptGenerator = new ReceiptGenerator(mockDataContext.Object);

            var inputBasket = "1 Book at 12,49\r\n1 Music CD at 14,99\r\n1 Chocolate bar at 0,85";

            // Act
            var receipt = await receiptGenerator.GenerateReceiptAsync(inputBasket);

            // Assert
            Assert.NotNull(receipt);
            Assert.Equal(29.83m, receipt.TotalPrice);
            Assert.Equal(1.50m, receipt.TotalTax);
        }

        [Fact]
        public async Task GenerateReceiptAsync_ReturnsNullForInvalidInputBasket()
        {
            // Arrange
            var mockDataContext = new Mock<IDataContext>();
            var mockDbSet = new Mock<DbSet<Item>>();
            var mockDbSetReceipt = new Mock<DbSet<Receipt>>();

            mockDataContext.Setup(m => m.Items).Returns(mockDbSet.Object);
            mockDataContext.Setup(m => m.Receipts).Returns(mockDbSetReceipt.Object);

            var receiptGenerator = new ReceiptGenerator(mockDataContext.Object);

            var inputBasket = "Invalid input basket";

            // Act
            var receipt = await receiptGenerator.GenerateReceiptAsync(inputBasket);

            // Assert
            Assert.Null(receipt);
        }

        [Fact]
        public void ParseInputBasket_ReturnsCorrectListOfItems()
        {
            // Arrange
            var receiptGenerator = new ReceiptGenerator(null); // DataContext is not used in this test

            var inputBasket = "1 Book at 12,49\r\n1 Music CD at 14,99\r\n1 Chocolate bar at 0,85";

            // Act
            var items = receiptGenerator.ParseInputBasket(inputBasket);

            // Assert
            Assert.NotNull(items);
            Assert.Equal(3, items.Count);

            // Check item details
            Assert.Equal("Book", items[0].Name);
            Assert.Equal(12.49m, items[0].Price);
            Assert.False(items[0].IsImported);
            Assert.Equal(ItemType.Book, items[0].Type);

            Assert.Equal("Music CD", items[1].Name);
            Assert.Equal(14.99m, items[1].Price);
            Assert.False(items[1].IsImported);
            Assert.Equal(ItemType.Other, items[1].Type);

            Assert.Equal("Chocolate bar", items[2].Name);
            Assert.Equal(0.85m, items[2].Price);
            Assert.False(items[2].IsImported);
            Assert.Equal(ItemType.Food, items[2].Type);
        }

    }

}