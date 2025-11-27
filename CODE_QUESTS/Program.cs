using Microsoft.AspNetCore.Identity; // This is the line you need to add
using Microsoft.EntityFrameworkCore;
using CODE_QUESTS.Data;
var builder = WebApplication.CreateBuilder(args);

// 1. Add the Database Connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Add Identity (the user system)
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>() // <-- ADD THIS
    .AddEntityFrameworkStores<ApplicationDbContext>();
// 3. Add MVC Controllers and Views
builder.Services.AddControllersWithViews();
builder.Services.ConfigureApplicationCookie(options =>
{
    // This tells the app that your custom login page is at /Account/Login
    options.LoginPath = "/Account/Login";
});
// END OF NEW BLOCK


var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// These are crucial for login to work
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // Needed for Identity's built-in pages

app.Run();