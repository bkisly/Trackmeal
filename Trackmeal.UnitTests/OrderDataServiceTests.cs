namespace Trackmeal.UnitTests
{
    public class OrderDataServiceTests
    {
        [Fact]
        public async Task SubmitOrderTest()
        {
            await using var context = Helpers.GetInMemoryContext("SubmitOrder_TestDb");
            var cartService = await Helpers.GetTestCartServiceAsync(context);
            var orderService = new OrderDataService(context, cartService);
            var entriesInCart = await cartService.GetItemsAsync();

            await orderService.AddItemAsync(new Order
            {
                Id = 1,
                Entries = entriesInCart.ToList(),
                OrderStatus = new OrderStatus { Name = "Submitted", Description = "Submitted order" }
            });

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

        [Fact]
        public async Task DeleteOrderTest()
        {
            await using var context = Helpers.GetInMemoryContext("DeleteOrder_TestDb");
            var cartService = await Helpers.GetTestCartServiceAsync(context);
            var orderService = new OrderDataService(context, cartService);

            await orderService.AddItemAsync(new Order
            {
                Id = 1,
                Entries = (await cartService.GetItemsAsync()).ToList(),
                OrderStatus = new OrderStatus { Name = "Submitted", Description = "Submitted order" }
            });

            Assert.NotEmpty(await orderService.GetItemsAsync());

            await cartService.AddProductAsync(1);
            await cartService.AddProductAsync(1);
            await cartService.AddProductAsync(2);
            await orderService.DeleteItemAsync(1);

            Assert.Empty(await orderService.GetItemsAsync());
            Assert.Equal(2, (await cartService.GetItemsAsync()).Length);
        }

        [Fact]
        public async Task ChangeOrderStateTest()
        {
            await using var context = Helpers.GetInMemoryContext("ChangeOrderState_TestDb");
            var orderService = await Helpers.GetTestOrderServiceAsync(context);
            var order = await orderService.GetItemByIdAsync(1);

            await orderService.NextStateAsync(1);
            Assert.Equal(2, order.OrderStatusId);

            for(int i = 0; i < 10; i++) await orderService.NextStateAsync(1);
            Assert.Equal(4, order.OrderStatusId);

            await orderService.PreviousStateAsync(1);
            Assert.Equal(3, order.OrderStatusId);

            for(int i = 0; i < 10; i++) await orderService.PreviousStateAsync(1);
            Assert.Equal(1, order.OrderStatusId);

            await orderService.SetStateAsync(1, 3);
            Assert.Equal(3, order.OrderStatusId);

            await Assert.ThrowsAsync<InvalidOperationException>(async () => await orderService.SetStateAsync(1, 20));
        }
    }
}
