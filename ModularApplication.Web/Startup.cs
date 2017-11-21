namespace ModularApplication.Web
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using ModularApplication.Shared;

    public class Startup
    {
        private readonly IConfiguration _configuration;
        private readonly ModulesStartup _modules; 

        public Startup(IConfiguration configuration)
        {
            this._configuration = configuration;
            this._modules = new ModulesStartup(configuration);
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            this._modules.ConfigureServices(services);
        }

        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            this._modules.Configure(app);
        }
    }
}