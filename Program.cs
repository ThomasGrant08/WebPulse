using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebPulse2023.Data;
using WebPulse2023.Services;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString),
    ServiceLifetime.Scoped); // Specify the scope explicitly

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient();

builder.Services.AddSingleton<CancellationTokenSource>();
builder.Services.AddScoped<IWebPingService, WebPingService>();
builder.Services.AddHostedService<WebPingBackgroundService>();
builder.Services.AddSignalR();
builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
