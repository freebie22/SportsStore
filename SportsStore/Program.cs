using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using SportsStore.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<StoreDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SportsStoreConnection")));
builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SportsStoreIdentityConnection")));
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();
builder.Services.AddScoped<IStoreRepository, EFStoreRepository>();
builder.Services.AddRazorPages();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();
builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<IOrderRepository, EFOrderRepository>();
builder.Services.AddServerSideBlazor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/error");
}
else
{
    app.UseDeveloperExceptionPage();
    app.UseStatusCodePages();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseDeveloperExceptionPage();
app.UseStatusCodePages();

app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "pagination",
    pattern: "Products/Page{productPage}", new { Controller = "Home", action = "Index", productPage = 1 }
    );
app.MapControllerRoute(
    name:"page",
    pattern: "Page{productPage:int}", new {Controller = "Home", action = "Index", productPage = 1}
    );
app.MapControllerRoute(
    name: "category",
    pattern: "{category}", new { Controller = "Home", action = "Index", productPage = 1 }
    );
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.MapBlazorHub();
app.MapFallbackToPage("/admin/{*catchall}", "/Admin/Index");

SeedData.EnsurePopulated(app);
await IdentitySeedData.EnsurePopulatedIdentity(app);

app.Run();
