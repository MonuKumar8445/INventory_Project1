using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using INventory_Project1.Interfaces;
using INventory_Project1.Areas.Identity.Data;
using INventory_Project1.Repository;
using INnventory_Project1.Models;
using INventory_Project1.Interface;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DbCon"); //?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbCons")));

builder.Services.AddScoped<IUnit, UnitRepo>();
builder.Services.AddScoped<ICategory, CategoryRepo>();
builder.Services.AddScoped<IBrand, BrandRepo>();
builder.Services.AddScoped<IProductProfile, ProductProfileRepo>();
builder.Services.AddScoped<IProductGroup, ProductGroupRepo>();
builder.Services.AddScoped<IProduct, ProductRepo>();
builder.Services.AddScoped<ISupplier, SupplierRepo>();
builder.Services.AddScoped<ICurrency, CurrencyRepo>();
builder.Services.AddScoped<IPurchaseOrder, PurchaseOrderRepo>();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddApplicationInsightsTelemetry();
//Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("");
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
app.Run();
