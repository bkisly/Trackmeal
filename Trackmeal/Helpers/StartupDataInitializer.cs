using Microsoft.AspNetCore.Identity;

namespace Trackmeal.Helpers
{
    public static class StartupDataInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider, string initialAdminPw)
        {
            var adminId = await EnsureUser(serviceProvider, initialAdminPw, "admin@trackmeal.com");
            await EnsureRole(serviceProvider, adminId, Constants.RoleNames.Administrator);
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider, string initialAdminPw, string initialAdminUsername)
        {
            // Get app's UserManager (class capable of adding or getting users in code)
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();
            if (userManager is null) throw new NullReferenceException("User manager cannot be null.");

            // Check if there's already an admin account, otherwise create one
            var admin = await userManager.FindByNameAsync(initialAdminUsername);

            if (admin is null)
            {
                admin = new IdentityUser
                {
                    UserName = initialAdminUsername,
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(admin, initialAdminPw);
            }

            // Admin will be null if the password is not strong enough
            if (admin is null)
                throw new InvalidDataException("The password for initial admin account is not strong enough.");

            return admin.Id;
        }

        private static async Task EnsureRole(IServiceProvider serviceProvider, string initialAdminId,
            string adminRoleName)
        {
            // Get app's RoleManager (class capable of creating and getting user roles)
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();
            if (roleManager is null) throw new NullReferenceException("Role manager cannot be null.");

            // Create admin role if it doesn't exist
            if (!await roleManager.RoleExistsAsync(adminRoleName))
                await roleManager.CreateAsync(new IdentityRole(adminRoleName));

            // Get the UserManager and assign the role to admin
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();
            if (userManager is null) throw new NullReferenceException("User manager cannot be null.");

            var admin = await userManager.FindByIdAsync(initialAdminId);
            if (admin is null) throw new InvalidDataException($"Couldn't find an admin with id {initialAdminId}");

            await userManager.AddToRoleAsync(admin, adminRoleName);
        }
    }
}
