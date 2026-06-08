using ConcertTickets.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();   
builder.Services.AddDbContext<ApplicationDbContext>(options =>   
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
        options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts(); 
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();   //  "sen kimsin?" — Authorization'dan ÖNCE gelmeli
app.UseAuthorization();    // "buna yetkin var mı?"

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Concerts}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages();   //  Razor Pages yönlendirmesi (Login/Register çalışsın diye)

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DbSeeder.SeedRolesAndAdminAsync(services);
}

app.Run();

app.Run();