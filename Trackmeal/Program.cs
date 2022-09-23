using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Trackmeal.Data;
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
builder.Services.AddScoped<ICartDataService, CartDataService>();
builder.Services.AddScoped<IModifiableDataService<Order>, OrderDataService>();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

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

app.Run();
