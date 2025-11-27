using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WEBNC.DataAccess.Repository.IRepository;
using WEBNC.DataAccess.Repository;
using WEBNC.Models;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.Services.AddControllersWithViews();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ──────────────────────────────────────────────────────────────
// IDENTITY + API + RAZOR PAGES UI CÙNG LÚC – HOÀN HẢO!
// ──────────────────────────────────────────────────────────────
builder.Services
    .AddIdentityApiEndpoints<ApplicationUser>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// 2 DÒNG BẮT BUỘC ĐỂ RAZOR PAGES UI (Login/Register custom) HOẠT ĐỘNG
builder.Services.AddAuthentication();   // ← FIX lỗi "Unable to find the required services"
builder.Services.AddRazorPages();       // ← Cho phép dùng trang Login/Register bạn tự viết

// Authorization + RoleManager
builder.Services.AddAuthorization();

// Các dịch vụ khác
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddSingleton<TimeProvider>(TimeProvider.System);

var app = builder.Build();

//đăng nhập
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    string email = "phamminhhuy0901tk@gmail.com";
    string password = "Abc123!@#";

    // tạo role nếu cần
    if (!await roleManager.RoleExistsAsync("Admin"))
        await roleManager.CreateAsync(new IdentityRole("Admin"));

    // nếu user tồn tại, xóa đi để reset lại
    var oldUser = await userManager.FindByEmailAsync(email);
    if (oldUser != null)
        await userManager.DeleteAsync(oldUser);

    // tạo user mới
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
    {
        await userManager.AddToRoleAsync(user, "Admin");
    }
    else
    {
        foreach (var err in result.Errors)
        {
            Console.WriteLine(err.Description);
        }
    }
}
//đăng nhập

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
app.UseAuthentication();
app.UseAuthorization();

// API Endpoints (token cho mobile/SPA)
app.MapGroup("/api/identity").MapIdentityApi<ApplicationUser>();

// Razor Pages – form đẹp bạn tự design
app.MapRazorPages();   // ← BẮT BUỘC CÓ DÒNG NÀY TRONG app.Build() nữa!

app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();