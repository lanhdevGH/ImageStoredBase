using ImageStoreBase.Api.Data;
using ImageStoreBase.Api.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

#region Tích hợp Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // Đọc cấu hình từ appsettings.json
    .Enrich.FromLogContext()
    .WriteTo.Console() // Ghi log ra console
    //.WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day) // Ghi log vào file (mỗi ngày 1 file)
    .CreateLogger();

builder.Host.UseSerilog(); // Đặt Serilog làm Logger chính
#endregion

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region DBContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
#endregion

#region Config_Identity
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<AppDbContext>();
builder.Services.Configure<IdentityOptions>(options =>
{
    // Thiết lập các tùy chọn khóa tài khoản (Lockout).
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5); // Thời gian khóa tài khoản mặc định là 5 phút.
    options.Lockout.MaxFailedAccessAttempts = 5; // Tài khoản sẽ bị khóa sau 5 lần đăng nhập sai.
    options.Lockout.AllowedForNewUsers = true; // Áp dụng chính sách khóa tài khoản cho cả người dùng mới.

    // Cấu hình các yêu cầu khi người dùng đăng nhập (SignIn).
    options.SignIn.RequireConfirmedPhoneNumber = false; // Không yêu cầu xác minh số điện thoại trước khi đăng nhập.
    options.SignIn.RequireConfirmedAccount = false; // Không yêu cầu xác nhận tài khoản trước khi đăng nhập.
    options.SignIn.RequireConfirmedEmail = false; // Không yêu cầu xác minh email trước khi đăng nhập.

    // Cấu hình chính sách mật khẩu (Password Policy).
    options.Password.RequiredLength = 8; // Độ dài mật khẩu tối thiểu là 8 ký tự.
    options.Password.RequireDigit = true; // Mật khẩu phải chứa ít nhất một chữ số (0-9).
    options.Password.RequireUppercase = true; // Mật khẩu phải chứa ít nhất một chữ cái in hoa (A-Z).

    // Cấu hình người dùng (User).
    options.User.RequireUniqueEmail = true; // Email phải là duy nhất trong hệ thống.
});
#endregion

#region CustomServices
builder.Services.AddTransient<DbInitializer>();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

#region SeedingData
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        Log.Information("Seeding data...");
        var dbInitializer = services.GetService<DbInitializer>();
        dbInitializer.SeedingData().Wait();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}
#endregion

app.Run();
