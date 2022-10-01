using Microsoft.AspNetCore.Identity;

namespace Trackmeal.UnitTests
{
    internal static class Helpers
    {
        public static ApplicationDbContext GetInMemoryContext(string databaseName)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName).Options;

            return new ApplicationDbContext(options);
        }

        public static async Task<IModifiableDataService<Product>> GetTestProductsServiceAsync(ApplicationDbContext context)
        {
            var productsService = new ProductsDataService(context);

            await productsService.AddItemAsync(new Product
            {
                Id = 1,
                Name = "Coffee",
                Price = 2.99M
            });

            await productsService.AddItemAsync(new Product
            {
                Id = 2,
                Name = "Pizza",
                Price = 2.99M
            });

            await productsService.AddItemAsync(new Product
            {
                Id = 3,
                Name = "Burger",
                Price = 2.99M
            });

            return productsService;
        }

        public static async Task<IIdentityCartDataService> GetTestCartServiceAsync(ApplicationDbContext context, bool empty = false)
        {
            var cartService = new CartDataService(context, await GetTestProductsServiceAsync(context));
            if (empty) return cartService;

            await cartService.AddProductAsync(1);
            await cartService.AddProductAsync(1);
            await cartService.AddProductAsync(2);
            await cartService.AddProductAsync(2);
            await cartService.AddProductAsync(2);
            await cartService.AddProductAsync(2);
            await cartService.AddProductAsync(2);
            await cartService.AddProductAsync(3);

            return cartService;
        }

        public static async Task<(IIdentityCartDataService CartService, IdentityUser[] TestUsers)> GetTestIdentityCartServiceAsync(ApplicationDbContext context,
            bool empty = false)
        {
            var cartService = await GetTestCartServiceAsync(context, true);
            if(empty) return (cartService, Array.Empty<IdentityUser>());

            var testUsers = GetTestUsers().ToArray();

            await cartService.AddProductAsync(1, testUsers[0]);
            await cartService.AddProductAsync(1, testUsers[0]);
            await cartService.AddProductAsync(2, testUsers[0]);

            await cartService.AddProductAsync(1, testUsers[1]);
            await cartService.AddProductAsync(2, testUsers[1]);
            await cartService.AddProductAsync(3, testUsers[1]);
            await cartService.AddProductAsync(3, testUsers[1]);

            await cartService.AddProductAsync(2, testUsers[2]);
            await cartService.AddProductAsync(3, testUsers[2]);

            return (cartService, testUsers);
        }

        public static async Task<IOrderDataService> GetTestOrderServiceAsync(ApplicationDbContext context)
        {
            var cartService = await GetTestCartServiceAsync(context);
            var orderService = new OrderDataService(context, cartService);
            await orderService.AddItemAsync(
                new Order
                {
                    Id = 1,
                    Entries = (await cartService.GetItemsAsync()).ToList(),
                    OrderStatus = new OrderStatus { Id = 1, Name = "Submitted" }
                }
            );

            return orderService;
        }

        private static IEnumerable<IdentityUser> GetTestUsers(uint count = 3)
        {
            for (uint i = 0; i < count; i++)
                yield return new IdentityUser { Id = (i + 1).ToString(), UserName = $"User {i + 1}" };
        }
    }
}
