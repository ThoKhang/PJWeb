using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WEBNC.DataAccess.Repository.IRepository;
using WEBNC.DataAccess.Repository;
using WEBNC.Models;

var builder = WebApplication.CreateBuilder(args);

// ════════════════════════════════════════════
// 1. CORS (cho API gọi từ JS)
// ════════════════════════════════════════════
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddControllersWithViews();

// ════════════════════════════════════════════
// 2. Database
// ════════════════════════════════════════════
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ════════════════════════════════════════════
// 3. Identity + API + Cookie Authentication
// ════════════════════════════════════════════
builder.Services
    .AddIdentityApiEndpoints<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// ⚡ BẮT BUỘC CHO COOKIE AUTH
builder.Services.ConfigureApplicationCookie(options =>
{
    // Đường dẫn login của Razor Pages Identity
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";

    // Thời gian cookie tồn tại
    options.ExpireTimeSpan = TimeSpan.FromDays(14);

    // Cookie sẽ được gửi trong mọi request
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.SlidingExpiration = true; // tự gia hạn khi người dùng hoạt động
});

// ⚡ BẮT BUỘC CHO RAZOR PAGES UI (Login/Logout…)
builder.Services.AddAuthentication();
builder.Services.AddRazorPages();

// Authorization + RoleManager
builder.Services.AddAuthorization();

// ════════════════════════════════════════════
// 4. Session
// ════════════════════════════════════════════
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// ════════════════════════════════════════════
// 5. DI
// ════════════════════════════════════════════
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddSingleton<TimeProvider>(TimeProvider.System);

var app = builder.Build();

// ─────────────────────────────────────────────────────
// 6. Auto tạo tài khoản Admin lần đầu
// ─────────────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    string email = "phamminhhuy0901tk@gmail.com";
    string password = "Abc123!@#";

    if (!await roleManager.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new IdentityRole("Admin"));

    var oldUser = await userManager.FindByEmailAsync(email);
    if (oldUser != null)
        await userManager.DeleteAsync(oldUser);

    var user = new ApplicationUser
    {
        UserName = email,
        Email = email,
        EmailConfirmed = true,
        idPhuongXa = "XP001",
        hoTen = "Phạm Minh Huy",
        soNha = "24 Bắc Đẩu"
    };

    var result = await userManager.CreateAsync(user, password);
    if (result.Succeeded)
        await userManager.AddToRoleAsync(user, "Admin");
}

// ════════════════════════════════════════════
// 7. Middleware
// ════════════════════════════════════════════
app.UseCors("AllowAll");

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();

// ⚡ COOKIE Authentication + Role Authorization
app.UseAuthentication();
app.UseAuthorization();

// ════════════════════════════════════════════
// 8. API Identity (cho token/mobile)
// ════════════════════════════════════════════
app.MapGroup("/api/identity").MapIdentityApi<ApplicationUser>();

// ════════════════════════════════════════════
// 9. Razor Pages Identity (UI Login & Register)
// ════════════════════════════════════════════
app.MapRazorPages();

// ════════════════════════════════════════════
// 10. MVC Controllers (web)
// ════════════════════════════════════════════
app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
