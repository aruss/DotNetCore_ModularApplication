# .NET Core Dependency Injection with Modules and Configuration File

https://blog.econduct.de/dotnet-core-dependency-injection-with-configuration-file/

[.NET Core 2.0](https://www.microsoft.com/net/learn/get-started/windows) has a build in dependency injection and it works well, but if you want to do it via configuration you have to hack around. So, my idea was to introduce a concept of modules that can be managed via configuration logic.

This idea is not new, I am big fan of [Autofac](https://autofac.org/) and Autofac has the concept of [modules](http://autofaccn.readthedocs.io/en/latest/configuration/modules.html).

> A module is a small class that can be used to bundle up a set of related components behind a ‘facade’ to simplify configuration and deployment. The module exposes a deliberate, restricted set of configuration parameters that can vary independently of the components used to implement the module.

I could use Autofac with all its goodies, but I decided to extend the built in DI mechanism with module feature. 

There are four files to enable this feature.

### IModule.cs
```c# 
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public interface IModule
{
    void ConfigureServices(
        IServiceCollection services,
        IConfiguration configuration);

    void Configure(IApplicationBuilder app);
}
```

### ModulesOption.cs
```c#
using System.Collections.Generic;

public class ModulesOptions
{
    public List<ModuleOptions> Modules { get; set; }
}
```

### ModuleOption.cs
```c#
public class ModuleOptions
{
    public string Type { get; set; }
}
```

### ModulesStartup.cs
```c#
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class ModulesStartup
{
    private readonly IEnumerable<IModule> _modules;
    private readonly IConfiguration _configuration;

    public ModulesStartup(IConfiguration configuration)
    {
        this._configuration = configuration ??
            throw new ArgumentNullException(nameof(configuration));

        ModulesOptions options = configuration.Get<ModulesOptions>();

        this._modules = options.Modules
            .Select(s =>
            {
                Type type = Type.GetType(s.Type);

                if (type == null)
                {
                    throw new TypeLoadException(
                        $"Cannot load type \"{s.Type}\"");
                }

                IModule module = (IModule)Activator.CreateInstance(type);
                return module;
            }
        );
    }

    public void ConfigureServices(IServiceCollection services)
    {
        foreach (IModule module in this._modules)
        {
            module.ConfigureServices(services, this._configuration);
        }
    }

    public void Configure(IApplicationBuilder app)
    {
        foreach (IModule module in this._modules)
        {
            module.Configure(app);
        }
    }
```

With this four classes you are ready to go. 
Lets say you want to load the `IEmailSender`, that is a part of default .NET Core template, from a module. You will need to create a `IEmailSender` implementation, something like this. 

### SomeFancyEmailSender.cs
```c#
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

public class SomeFancyEmailSender : IEmailSender
{
    private readonly ILogger<SomeFancyEmailSender> _logger;

    public SomeFancyEmailSender(ILogger<SomeFancyEmailSender> logger)
    {
        this._logger = logger;
    }

    public async Task SendEmailAsync(
        string email, 
        string subject,
        string message)
    {
        // Call some fancy API here 
        this._logger
            .LogInformation("SomeFancyEmailSender.SendEmailAsync...");
    }
}
```

And you will need a module class. 

### 

```c#
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
```

To make it whole thing work you we need to integrate it in `Startup.cs` and `appsettings.json` files. 

### Startup.cs

```c#
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public class Startup
{
    private readonly ModulesStartup _modules;

    public Startup(IConfiguration configuration)
    {
        this._modules = new ModulesStartup(configuration);
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc();
        this._modules.ConfigureServices(services);
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
        app.UseMvcWithDefaultRoute();
        this._modules.Configure(app);
    }
}
```

### appsettings.json
```json
{
  "Modules": [
    { "Type": "SomeFancyModules.SomeFancyEmailSenderModule, SomeFancyModules" }
  ]
}
```

You can find the code used in this post on [GitHub](https://github.com/aruss/DotNetCore_ModularApplication)
