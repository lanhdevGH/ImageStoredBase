namespace ImageStoreBase.Api.Configurations
{
    public static class IdentityServerConfig
    {
        public static void ConfigIdentityServer(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddIdentityServer(options =>
            //    {
            //        options.Events.RaiseErrorEvents = true;
            //        options.Events.RaiseInformationEvents = true;
            //        options.Events.RaiseFailureEvents = true;
            //        options.Events.RaiseSuccessEvents = true;
            //    })
            //    .AddInMemoryClients(Config.Clients)
            //    .AddInMemoryIdentityResources(Config.IdentityResources)
            //    .AddInMemoryApiScopes(Config.ApiResources);
        }
    }
}
