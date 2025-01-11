using ImageStoreBase.Api.Data;
using ImageStoreBase.Api.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace ImageStoreBase.Api.Configurations
{
    public static class IdentityConfig
    {
        public static void ConfigIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            services.Configure<IdentityOptions>(options =>
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
                options.Password.RequiredUniqueChars = 1;

                // Cấu hình người dùng (User).
                options.User.RequireUniqueEmail = true; // Email phải là duy nhất trong hệ thống.
            });
        }
    }
}
