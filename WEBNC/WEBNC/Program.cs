using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WEBNC.DataAccess.Repository.IRepository;
using WEBNC.DataAccess.Repository;
using WEBNC.Models;

var builder = WebApplication.CreateBuilder(args);

// ════════════════════════════════════════════
// 1. CORS
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
// 3. Identity + Cookie Authentication
// ════════════════════════════════════════════
builder.Services
    .AddIdentityApiEndpoints<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// BẮT MVC DÙNG COOKIE Identity (QUAN TRỌNG)
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
});

//Cấu hình cookie đăng nhập
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Identity/Account/Login";
    options.LogoutPath = "/Identity/Account/Logout";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";

    options.ExpireTimeSpan = TimeSpan.FromDays(14);
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.SlidingExpiration = true;
});

// Razor Pages Identity UI
builder.Services.AddRazorPages();

// Authorization
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

// ════════════════════════════════════════════
// 6. Auto tạo tài khoản Admin lần đầu
// ════════════════════════════════════════════
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

// Authentication + Authorization
app.UseAuthentication();
app.UseAuthorization();

// ════════════════════════════════════════════
// 8. API Identity
// ════════════════════════════════════════════
app.MapGroup("/api/identity").MapIdentityApi<ApplicationUser>();

// ════════════════════════════════════════════
// 9. Razor Pages Identity (Login/Register)
// ════════════════════════════════════════════
app.MapRazorPages();

// ════════════════════════════════════════════
// 10. MVC Controllers
// ════════════════════════════════════════════
app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
