namespace SomeFancyModules
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using ModularApplication.Shared;

    public class SomeFancyEmailSenderModule : IModule
    {
        public void ConfigureServices(
            IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<IEmailSender, SomeFancyEmailSender>(); 
        }

        public void Configure(IApplicationBuilder app)
        {
            // Configure services 
        }
    }
}
