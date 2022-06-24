using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OrchardCore.Commerce.Web;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration) => _configuration = configuration;

    public void ConfigureServices(IServiceCollection services) =>
        services.AddOrchardCms(builder => builder
            .ConfigureUITesting(_configuration, enableShortcutsDuringUITesting: true)
            .AddSetupFeatures("OrchardCore.AutoSetup"));

    public void Configure(IApplicationBuilder app, IHostEnvironment env)
    {
        app.UseStaticFiles();

        app.UseOrchardCore();
    }
}
