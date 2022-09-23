﻿using Trackmeal.Models;
using Trackmeal.Services;

namespace Trackmeal.UnitTests
{
    public class CartDataServiceTests
    {
        [Fact]
        public async Task AddNonExistingProductTest()
        {
            await using var context = Helpers.GetInMemoryContext("AddNonExistingProduct_Test");
            var productsService = await Helpers.GetTestProductsServiceAsync(context);
            var cartService = new CartDataService(context, productsService);

            await cartService.AddProductAsync(1);
            await cartService.AddProductAsync(2);
            await cartService.AddProductAsync(3);

            var entries = await cartService.GetItemsAsync();

            // Check the amount of added cart entries
            Assert.Equal(3, entries.Length);

            // Check amount values for each product and ensure that references are not null
            foreach (var cartEntry in entries)
            {
                Assert.Equal(1, cartEntry.Amount);
                Assert.NotNull(cartEntry.Product);
            }

            // Check if referenced products are valid
            for(int i = 0; i < entries.Length; i++) Assert.Equal(i + 1, entries[i].Product.Id);
        }

        [Fact]
        public async Task AddExistingProductTest()
        {
            await using var context = Helpers.GetInMemoryContext("AddExistingProduct_Test");
            var productsService = await Helpers.GetTestProductsServiceAsync(context);
            var cartService = new CartDataService(context, productsService);

            await cartService.AddProductAsync(1);
            await cartService.AddProductAsync(1);
            await cartService.AddProductAsync(2);
            await cartService.AddProductAsync(2);
            await cartService.AddProductAsync(2);
            await cartService.AddProductAsync(3);

            var entries = await cartService.GetItemsAsync();

            // Check the amount of added cart entries
            Assert.Equal(3, entries.Length);

            // Check product amount values
            Assert.Equal(2, entries[0].Amount);
            Assert.Equal(3, entries[1].Amount);
            Assert.Equal(1, entries[2].Amount);

            // Check if references to the products are not null
            foreach(var cartEntry in entries) Assert.NotNull(cartEntry.Product);

            // Check if referenced products are valid
            for (int i = 0; i < entries.Length; i++) Assert.Equal(i + 1, entries[i].Product.Id);
        }       

        [Fact]
        public async Task DeleteEntryWithSeveralProductsTest()
        {
            await using var context = Helpers.GetInMemoryContext("DeleteEntryWithMultiple_Test");
            var productsService = await Helpers.GetTestProductsServiceAsync(context);
            var cartService = new CartDataService(context, productsService);

            await cartService.AddProductAsync(1);
            await cartService.AddProductAsync(1);
            await cartService.AddProductAsync(2);

            await cartService.RemoveProductAsync(1);
            var entries = await cartService.GetItemsAsync();

            // Check if amounts are properly reduced
            Assert.Equal(2, entries.Length);
            Assert.Equal(1, entries[0].Amount);
        }

        [Fact]
        public async Task DeleteEntryWithSingleProductTest()
        {
            await using var context = Helpers.GetInMemoryContext("DeleteEntryWithSingle_Test");
            var productsService = await Helpers.GetTestProductsServiceAsync(context);
            var cartService = new CartDataService(context, productsService);

            await cartService.AddProductAsync(1);
            await cartService.AddProductAsync(2);
            await cartService.AddProductAsync(3);

            await cartService.RemoveProductAsync(1);
            var entries = await cartService.GetItemsAsync();

            // Check if the whole entry has been removed
            Assert.Equal(2, entries.Length);
            foreach (var cartEntry in entries)
                Assert.NotEqual(1, cartEntry.Product.Id);
        }

        [Fact]
        public async Task ClearCartTest()
        {
            await using var context = Helpers.GetInMemoryContext("ClearCart_Test");
            var productsService = await Helpers.GetTestProductsServiceAsync(context);
            var cartService = new CartDataService(context, productsService);

            await cartService.AddProductAsync(1);
            await cartService.AddProductAsync(1);
            await cartService.AddProductAsync(1);
            await cartService.AddProductAsync(2);
            await cartService.AddProductAsync(2);
            await cartService.AddProductAsync(3);
            await cartService.AddProductAsync(3);
            await cartService.AddProductAsync(3);

            var entries = await cartService.GetItemsAsync();

            Assert.NotEmpty(entries);

            await cartService.ClearCartAsync();
            entries = await cartService.GetItemsAsync();

            Assert.Empty(entries);
        }

        [Fact]
        public async Task GetCartEntriesAfterSubmittedOrder()
        {
            await using var context = Helpers.GetInMemoryContext("GetEntriesAfterSubmitted_TestDb");
            var cartService = await Helpers.GetTestCartServiceAsync(context);
            var orderService = new OrderDataService(context);
            var entriesInCart = await cartService.GetItemsAsync();

            await orderService.AddItemAsync(new Order
            {
                Id = 1,
                Entries = entriesInCart.ToList()
            });

            await cartService.ClearCartAsync();

            Assert.Empty(await cartService.GetItemsAsync());
        }

        [Fact]
        public async Task AddEntryAfterSubmittedOrder()
        {
            await using var context = Helpers.GetInMemoryContext("AddEntriesAfterSubmitted_TestDb");
            var cartService = await Helpers.GetTestCartServiceAsync(context);
            var orderService = new OrderDataService(context);

            await cartService.AddProductAsync(1);
            await orderService.AddItemAsync(new Order { Entries = (await cartService.GetItemsAsync()).ToList() });
            await cartService.ClearCartAsync();

            await cartService.AddProductAsync(1);
            var newEntries = await cartService.GetItemsAsync();

            Assert.NotEmpty(newEntries);
            Assert.Single(newEntries);
            Assert.Equal(1, newEntries.First().Product.Id);
            Assert.Null(newEntries.First().OrderId);
        }
    }
}
