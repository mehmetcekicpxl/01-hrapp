using HrApp.Data;
using HrApp.Repositories;
using HrApp.Repositories.Interfaces;
using HrApp.Services;
using HrApp.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("HrAppConnection"));
});

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

//services registeren
builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

builder.Services.AddControllersWithViews();


// openid connect
builder.Services
        .AddAuthentication(options =>
        {
            options.DefaultScheme = "Cookies";
            options.DefaultChallengeScheme = "oidc";
        })
        .AddCookie("Cookies")
        .AddOpenIdConnect("oidc", options =>
        {
            options.Authority = "https://localhost:5001"; // IdentityServer URL
            options.ClientId = "mvc_client";
            options.ClientSecret = "secret";
            options.ResponseType = "code";
            options.SaveTokens = true;
            options.Scope.Add("profile");
            options.Scope.Add("email");
        })
        // 2. Google yap?land?rmas?n? buraya ekliyoruz
        .AddGoogle(options =>
        {
            // S?navda hocan?n verece?i de?erler buraya gelecek
            options.ClientId = "HOCANIN_VERDIGI_GOOGLE_ID";
            options.ClientSecret = "HOCANIN_VERDIGI_GOOGLE_SECRET";
        });

var app = builder.Build();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.EnsureDbMigrated();

app.Run();
