using ImageStoreBase.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace ImageStoreBase.Api.Configurations
{
    public static class DBContextConfig
    {
        public static void ConfigDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
        }
    }
}
