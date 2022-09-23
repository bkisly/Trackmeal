namespace Trackmeal.UnitTests
{
    public class OrderDataServiceTests
    {
        [Fact]
        public async Task SubmitOrderTest()
        {
            await using var context = Helpers.GetInMemoryContext("SubmitOrder_TestDb");
            var cartService = await Helpers.GetTestCartServiceAsync(context);
            var orderService = new OrderDataService(context);
            var entriesInCart = await cartService.GetItemsAsync();

            await orderService.AddItemAsync(new Order
            {
                Id = 1,
                Entries = entriesInCart.ToList()
            });

            await cartService.ClearCartAsync();

            var order = await orderService.GetItemByIdAsync(1);
            
            // Check if order-specific properties are correct
            Assert.Equal(1, order.Id);
            Assert.True(DateTime.Now - order.DateOrdered < TimeSpan.FromSeconds(1));

            // Check if entries list for order isn't gone after clearing the cart
            Assert.NotNull(order.Entries);
            Assert.NotEmpty(order.Entries);
            Assert.Equal(entriesInCart.Length, order.Entries.Count);

            // Check entries list contents
            foreach (var (entryInOrder, entryFromCart) in order.Entries.Zip(entriesInCart))
                Assert.Equivalent(entryFromCart, entryInOrder);
        }
    }
}
