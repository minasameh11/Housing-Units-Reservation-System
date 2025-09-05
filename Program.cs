using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;
using System.IO;
using ProjectName.Models; // غيّر ProjectName للـ namespace الفعلي بتاعك

var builder = WebApplication.CreateBuilder(args);

// -------------------- Configuration --------------------
IConfiguration configuration = builder.Configuration;

// -------------------- Services --------------------
// MVC
builder.Services.AddControllersWithViews();

// DbContext - SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(configuration.GetConnectionString("DefaultConnection") ?? "Data Source=app.db"));

// لو عايز تتحكم في حجم ملفات الرفع (مثلاً صور كبيرة)، تقدر تغير القيم دي
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 20 * 1024 * 1024; // 20 MB
});

// اختياري: لو عايز تستخدم IHttpContextAccessor في أي سيرفس
builder.Services.AddHttpContextAccessor();

// -------------------- Build app --------------------
var app = builder.Build();

// -------------------- Ensure DB + Migrations on startup --------------------
// هينفّذ المايجريشنات لو في، ويعمل قاعدة البيانات لو مش موجودة.
// مفيد في التطوير، لو في سيرفر إنتاج تأكد من سياسة المايجريشن المناسبة.
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var db = services.GetRequiredService<AppDbContext>();
        db.Database.Migrate(); // يطبّق المايجريشنات
    }
    catch (Exception ex)
    {
        // هنا تقدر تسجل الاستثناء أو تطبعه للوغ
        Console.WriteLine("Error applying migrations: " + ex.Message);
    }

    // تأكد وجود مجلد الصور داخل wwwroot
    var env = services.GetRequiredService<IWebHostEnvironment>();
    var imagesPath = Path.Combine(env.WebRootPath ?? "wwwroot", "images");
    if (!Directory.Exists(imagesPath))
    {
        Directory.CreateDirectory(imagesPath);
    }
}

// -------------------- Middleware --------------------
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // مهم عشان يتمكن المشروع من تحميل الصور، css، js من wwwroot

app.UseRouting();

// لو ناوي تضيف Authentication/Authorization حطهم هنا:
// app.UseAuthentication();
// app.UseAuthorization();

// -------------------- Endpoints / Routes --------------------
// افتراضي: Unit controller صفحة Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.Run();
