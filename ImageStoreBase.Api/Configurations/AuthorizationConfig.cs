using ImageStoreBase.Api.Handles.Auth;
using ImageStoreBase.Common.Define;

namespace ImageStoreBase.Api.Configurations
{
    public static class AuthorizationConfig
    {
        public static void ConfigAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                        policy.Requirements.Add(new ClaimRequirement(
                            nameof(MyApplicationDefine.Function.SYSTEM),
                            nameof(MyApplicationDefine.Command.CREATE))));
            });
        }
    }
}
