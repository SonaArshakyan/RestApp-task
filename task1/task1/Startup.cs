using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestApp.Interfaces;
using ThirdParty;

namespace RestApp;

public class Startup
{
    IConfigurationRoot Configuration { get; }

    public Startup()
    {
        var builder = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json");
        Configuration = builder.Build();
    }    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IConfigurationRoot>(Configuration);
        services.AddSingleton<IRestClient,RestClient>();
        services.AddSingleton<ILogger, Logger>();
        services.AddSingleton<IRestApp,RestAppClassThatUsesRestClient>();
        AppSettings options = new();
        Configuration.GetSection(nameof(AppSettings)).Bind(options);
        services.AddSingleton(options);
    }
}
