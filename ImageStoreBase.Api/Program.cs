using ImageStoreBase.Api.Configurations;
using ImageStoreBase.Api.Data;
using ImageStoreBase.Api.Data.Entities;
using ImageStoreBase.Api.Handles.Auth;
using ImageStoreBase.Api.Infrastructure;
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
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration) // Đọc cấu hình từ appsettings.json
    .Enrich.FromLogContext()
    .WriteTo.Console() // Ghi log ra console
                       //.WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day) // Ghi log vào file (mỗi ngày 1 file)
    .CreateLogger();


builder.Host.UseSerilog(); // Đặt Serilog làm Logger chính

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureSwagger();
builder.Services.ConfigDbContext(builder.Configuration);
builder.Services.ConfigIdentity();
builder.Services.ConfigAuthentication(builder.Configuration);
builder.Services.ConfigAuthorization();
builder.Services.ConfigCors();

#region DI Services
builder.Services.AddSingleton<IAuthorizationHandler, ClaimRequirementHandle>();
builder.Services.AddScoped<TokenProvider>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddTransient<DbInitializer>();
builder.Services.AddScoped<RoleService>();
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
