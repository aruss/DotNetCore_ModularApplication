namespace ModularApplication.Web
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using ModularApplication.Shared;

    public class SmtpEmailSenderModule : IModule
    {
        public void ConfigureServices(
            IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddScoped<IEmailSender, SmtpEmailSender>();
        }

        public void Configure(IApplicationBuilder app)
        {
            
        }
    }
}
