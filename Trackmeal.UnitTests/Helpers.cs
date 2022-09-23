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

        public static async Task<ICartDataService> GetTestCartServiceAsync(ApplicationDbContext context)
        {
            var cartService = new CartDataService(context, await GetTestProductsServiceAsync(context));

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
    }
}
