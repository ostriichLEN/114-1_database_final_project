using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using _114_1_database_final_project.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// ==========================================
// 1. 原本的資料庫連線 (Character1Context)
// ↓↓↓ 您原本缺少的正是這一行 ↓↓↓
// ==========================================
builder.Services.AddDbContext<Character1Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ==========================================
// 2. 登入系統的資料庫連線 (AppIdentityContext)
// ==========================================
builder.Services.AddDbContext<AppIdentityContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ==========================================
// 3. 啟用 Identity 服務 (登入/註冊功能)
// ==========================================
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    // 密碼強度設定 (方便測試用，可自行調整)
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 4;
})
    .AddEntityFrameworkStores<AppIdentityContext>();

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

// 啟用驗證與授權
app.UseAuthentication();
app.UseAuthorization();

// 設定路由
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// 啟用 Identity 的頁面 (Razor Pages)
app.MapRazorPages();

app.Run();