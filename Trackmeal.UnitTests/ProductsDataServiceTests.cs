using Microsoft.EntityFrameworkCore;
using Trackmeal.Data;
using Trackmeal.Models;
using Trackmeal.Services;

namespace Trackmeal.UnitTests
{
    public class ProductsDataServiceTests
    {
        [Fact]
        public async Task AddItemTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "GetItems_TestDb").Options;

            Product firstProduct = new() { Id = 1, Name = "Test product", Price = 2.99M };
            Product secondProduct = new()
            {
                Id = 2,
                Name = "Test product with description",
                Description = "Test description",
                Price = 14.99M
            };

            await using (var writeContext = new ApplicationDbContext(options))
            {
                var writeService = new ProductsDataService(writeContext);
                await writeService.AddItemAsync(firstProduct);
                await writeService.AddItemAsync(secondProduct);
            }

            await using var readContext = new ApplicationDbContext(options);
            var readService = new ProductsDataService(readContext);
            var items = await readService.GetItemsAsync();

            Assert.Equal(2, items.Length);
            Assert.Equal(firstProduct.Name, (await readService.GetItemByIdAsync(1)).Name);
            Assert.Equal(secondProduct.Name, (await readService.GetItemByIdAsync(2)).Name);
        }

        [Fact]
        public async Task UpdateItemTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "UpdateItems_TestDb").Options;

            Product firstProduct = new() { Id = 1, Name = "Test product", Price = 2.99M };
            Product secondProduct = new()
            {
                Id = 2,
                Name = "Test product with description",
                Description = "Test description",
                Price = 14.99M
            };

            Product editedProduct = new() { Id = 3, Name = "Updated product", Price = 3.49M };

            await using (var writeContext = new ApplicationDbContext(options))
            {
                var writeService = new ProductsDataService(writeContext);
                await writeService.AddItemAsync(firstProduct);
                await writeService.AddItemAsync(secondProduct);
                await writeService.EditItemAsync(1, editedProduct);

            }

            await using var readContext = new ApplicationDbContext(options);
            var readService = new ProductsDataService(readContext);
            var product = await readService.GetItemByIdAsync(1);

            Assert.Equal(1, product.Id);
            Assert.Equal("Updated product", product.Name);
            Assert.Equal(3.49M, product.Price);
        }

        [Fact]
        public async Task DeleteItemTest()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "DeleteItems_TestDb").Options;

            Product firstProduct = new() { Id = 1, Name = "Test product", Price = 2.99M };
            Product secondProduct = new()
            {
                Id = 2,
                Name = "Test product with description",
                Description = "Test description",
                Price = 14.99M
            };

            await using (var writeContext = new ApplicationDbContext(options))
            {
                var writeService = new ProductsDataService(writeContext);
                await writeService.AddItemAsync(firstProduct);
                await writeService.AddItemAsync(secondProduct);

                await writeService.DeleteItemAsync(2);
            }

            await using var readContext = new ApplicationDbContext(options);
            var readService = new ProductsDataService(readContext);
            var products = await readService.GetItemsAsync();

            Assert.Single(products);
            await Assert.ThrowsAsync<InvalidOperationException>(async () => await readService.GetItemByIdAsync(2));
        }
    }
}
