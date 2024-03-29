using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Trackmeal.Data;
using Trackmeal.Helpers;
using Trackmeal.Hubs;
using Trackmeal.Models;
using Trackmeal.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Test data services
/*builder.Services.AddSingleton<IModifiableDataService<Product>, TestProductsDataService>();
builder.Services.AddSingleton<ICartDataService, TestCartDataService>();
builder.Services.AddSingleton<IModifiableDataService<Order>, TestOrderDataService>();*/

// Real data services
builder.Services.AddScoped<IModifiableDataService<Product>, ProductsDataService>();
builder.Services.AddScoped<IIdentityCartDataService, CartDataService>();
builder.Services.AddScoped<IOrderDataService, OrderDataService>();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

// By default require authenticated users
builder.Services.AddAuthorization(options =>
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build());

var app = builder.Build();

// Seed the database with initial data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();

    var initialAdminPw = builder.Configuration.GetValue<string>("InitialAdminPassword");
    await StartupDataInitializer.Initialize(services, initialAdminPw);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapAreaControllerRoute(
    name: "ManageArea",
    areaName: "Manage",
    pattern: "Manage/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.MapHub<OrderStatusHub>("/statusHub");

app.Run();
