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
        public async Task GetCartEntriesAfterSubmittedOrderTest()
        {
            await using var context = Helpers.GetInMemoryContext("GetEntriesAfterSubmitted_TestDb");
            var cartService = await Helpers.GetTestCartServiceAsync(context);
            var orderService = new OrderDataService(context, cartService);
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
        public async Task AddEntryAfterSubmittedOrderTest()
        {
            await using var context = Helpers.GetInMemoryContext("AddEntriesAfterSubmitted_TestDb");
            var cartService = await Helpers.GetTestCartServiceAsync(context);
            var orderService = new OrderDataService(context, cartService);

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

        [Fact]
        public async Task RemoveEntryAfterSubmittedOrderTest()
        {
            await using var context = Helpers.GetInMemoryContext("RemoveEntriesAfterSubmitted_TestDb");
            var cartService = await Helpers.GetTestCartServiceAsync(context);
            var orderService = new OrderDataService(context, cartService);

            await cartService.AddProductAsync(1);
            await orderService.AddItemAsync(new Order { Entries = (await cartService.GetItemsAsync()).ToList() });

            await cartService.AddProductAsync(1);
            var exceptionResult = await Record.ExceptionAsync(async () => await cartService.RemoveProductAsync(1));

            Assert.Null(exceptionResult);
            Assert.Empty(await cartService.GetItemsAsync());
        }

        [Fact]
        public async Task GetIdentityEntriesTest()
        {
            await using var context = Helpers.GetInMemoryContext("GetIdentityEntries_TestDb");
            var (cartService, users) = await Helpers.GetTestIdentityCartServiceAsync(context);

            var firstUserEntries = await cartService.GetItemsAsync(users[0]);
            var secondUserEntries = await cartService.GetItemsAsync(users[1]);
            var thirdUserEntries = await cartService.GetItemsAsync(users[2]);
            var allEntries = await cartService.GetAllEntries();

            Assert.Equal(2, firstUserEntries.Length);
            Assert.Equal(3, secondUserEntries.Length);
            Assert.Equal(2, thirdUserEntries.Length);

            Assert.Equal(7, allEntries.Length);
        }

        [Fact]
        public async Task ModifyIdentityEntriesTest()
        {
            await using var context = Helpers.GetInMemoryContext("ModifyIdentityEntries_TestDb");
            var (cartService, users) = await Helpers.GetTestIdentityCartServiceAsync(context, true);

            await cartService.AddProductAsync(1, users[0]);
            await cartService.AddProductAsync(2, users[0]);
            await cartService.AddProductAsync(3, users[0]);
            await cartService.AddProductAsync(3, users[0]);

            await cartService.AddProductAsync(1, users[1]);
            await cartService.AddProductAsync(1, users[1]);
            await cartService.AddProductAsync(1, users[1]);

            await cartService.AddProductAsync(2, users[2]);
            await cartService.AddProductAsync(2, users[2]);
            await cartService.AddProductAsync(3, users[2]);

            var firstUserEntries = await cartService.GetItemsAsync(users[0]);
            var secondUserEntries = await cartService.GetItemsAsync(users[1]);
            var thirdUserEntries = await cartService.GetItemsAsync(users[2]);

            Assert.Equal(3, firstUserEntries.Length);
            Assert.Single( secondUserEntries);
            Assert.Equal(2, thirdUserEntries.Length);
            Assert.Equal(6, (await cartService.GetAllEntries()).Length);

            await cartService.RemoveProductAsync(1, users[0]);
            await cartService.RemoveProductAsync(2, users[0]);
            await cartService.RemoveProductAsync(3, users[0]);

            await cartService.RemoveProductAsync(1, users[1]);
            await cartService.RemoveProductAsync(1, users[1]);
            await cartService.RemoveProductAsync(1, users[1]);

            await cartService.RemoveProductAsync(2, users[2]);

            firstUserEntries = await cartService.GetItemsAsync(users[0]);
            secondUserEntries = await cartService.GetItemsAsync(users[1]);
            thirdUserEntries = await cartService.GetItemsAsync(users[2]);

            Assert.Single(firstUserEntries);
            Assert.Equal(1, firstUserEntries.First().Amount);

            Assert.Empty(secondUserEntries);

            Assert.Equal(2, thirdUserEntries.Length);
            Assert.True(thirdUserEntries.All(entry => entry.Amount == 1));
        }

        [Fact]
        public async Task ClearIdentityEntriesTest()
        {
            await using var context = Helpers.GetInMemoryContext("ClearIentityEntries_TestDb");
            var (cartService, users) = await Helpers.GetTestIdentityCartServiceAsync(context, true);

            await cartService.AddProductAsync(1, users[0]);
            await cartService.AddProductAsync(2, users[0]);
            await cartService.AddProductAsync(3, users[0]);
            await cartService.AddProductAsync(3, users[0]);

            await cartService.AddProductAsync(1, users[1]);
            await cartService.AddProductAsync(1, users[1]);
            await cartService.AddProductAsync(1, users[1]);

            await cartService.AddProductAsync(2, users[2]);
            await cartService.AddProductAsync(2, users[2]);
            await cartService.AddProductAsync(3, users[2]);

            await cartService.ClearCartAsync(users[0]);

            var firstUserEntries = await cartService.GetItemsAsync(users[0]);
            var secondUserEntries = await cartService.GetItemsAsync(users[1]);
            var thirdUserEntries = await cartService.GetItemsAsync(users[2]);

            Assert.Empty(firstUserEntries);
            Assert.Single(secondUserEntries);
            Assert.Equal(2, thirdUserEntries.Length);
            Assert.Equal(3, (await cartService.GetAllEntries()).Length);

            await cartService.ClearCartAsync(users[1]);
            firstUserEntries = await cartService.GetItemsAsync(users[0]);
            secondUserEntries = await cartService.GetItemsAsync(users[1]);
            thirdUserEntries = await cartService.GetItemsAsync(users[2]);

            Assert.Empty(firstUserEntries);
            Assert.Empty(secondUserEntries);
            Assert.Equal(2, thirdUserEntries.Length);
            Assert.Equal(2, (await cartService.GetAllEntries()).Length);

            await cartService.ClearCartAsync(users[2]);
            firstUserEntries = await cartService.GetItemsAsync(users[0]);
            secondUserEntries = await cartService.GetItemsAsync(users[1]);
            thirdUserEntries = await cartService.GetItemsAsync(users[2]);

            Assert.Empty(firstUserEntries);
            Assert.Empty(secondUserEntries);
            Assert.Empty(thirdUserEntries);
            Assert.Empty(await cartService.GetAllEntries());
        }
    }
}
