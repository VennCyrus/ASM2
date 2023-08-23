using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using FPTBOOK_STORE.Areas.Identity.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<FPTBOOK_STOREIdentityDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("FPTBOOK_STOREIdentityDbContextConnection") ?? throw new InvalidOperationException("Connection string 'FPTBOOK_STOREIdentityDbContextConnection' not found.")));

builder.Services.AddDefaultIdentity<FPTBOOKUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<FPTBOOK_STOREIdentityDbContext>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".MovieTicket.Session";
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

using (var scope = app.Services.CreateScope()){
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<FPTBOOKUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    await ContextSeed.SeedRolesAsync(userManager, roleManager);
    await ContextSeed.SeedAdminAsync(userManager, roleManager);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthentication();;

app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=}/{action=}/{id}");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Book}/{action=BookHome}/");



app.MapRazorPages();
app.Run();
