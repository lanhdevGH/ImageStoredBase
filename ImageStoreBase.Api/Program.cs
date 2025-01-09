using ImageStoreBase.Api.Data;
using ImageStoreBase.Api.Data.Entities;
using ImageStoreBase.Api.Handles.Auth;
using ImageStoreBase.Api.Services;
using ImageStoreBase.Common.Define;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

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
#region Config Swagger
builder.Services.AddSwaggerGen(options =>
{
    // Cấu hình để hỗ trợ Bearer Authentication
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Nhập 'Bearer' [khoảng trắng] và token của bạn trong ô bên dưới.\n\nVí dụ: Bearer abc123"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
});
#endregion

#region DBContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
#endregion

#region Config_Identity
builder.Services.AddIdentity<User, Role>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

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
    options.Password.RequiredUniqueChars = 1;

    // Cấu hình người dùng (User).
    options.User.RequireUniqueEmail = true; // Email phải là duy nhất trong hệ thống.
});
#endregion

#region Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"], // Định nghĩa trong appsettings.json
            //ValidAudience = builder.Configuration["JwtSettings:Audience"], // Định nghĩa trong appsettings.json
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"])) // Key bí mật
        };

        // Đảm bảo mọi claim từ JWT đều được ánh xạ
        options.MapInboundClaims = false;
    });
#endregion

#region Authorization
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy =>
        policy.Requirements.Add(new ClaimRequirement(nameof(MyApplicationDefine.Function.SYSTEM), nameof(MyApplicationDefine.Command.CREATE))));
});

#endregion

#region Config Identity Server
//builder.Services.AddIdentityServer(options =>
//{
//    options.Events.RaiseErrorEvents = true;
//    options.Events.RaiseInformationEvents = true;
//    options.Events.RaiseFailureEvents = true;
//    options.Events.RaiseSuccessEvents = true;
//})
//    .AddInMemoryClients(Config.Clients)
//    .AddInMemoryIdentityResources(Config.IdentityResources)
//    .AddInMemoryApiScopes(Config.ApiResources);
#endregion

#region DI Services
// Authorization
// Đăng ký AutoMapper
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddTransient<DbInitializer>();
// CustomService
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<IAuthorizationHandler, ClaimRequirementHandle>();
#endregion

#region CORS
// Thêm vào phần services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.PreSerializeFilters.Add((document, request) =>
        {
            var paths = document.Paths.ToDictionary(item => item.Key.ToLowerInvariant(), item => item.Value);
            document.Paths.Clear();
            foreach (var pathItem in paths)
            {
                document.Paths.Add(pathItem.Key, pathItem.Value);
            }
        });
    });
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "REST API V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowAll");

#region SeedingData
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        Log.Information("Seeding data...");
        var dbInitializer = services.GetService<DbInitializer>();
        dbInitializer.SeedingData().Wait();
        Log.Information("Done Seeding data");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}
#endregion

app.Run();
