using Microsoft.Extensions.DependencyInjection;
using RestApp;
using RestApp.Interfaces;
using RestApp.Models;

public class Program
{
    public static  void Main(string[] args)
    {
        IServiceCollection services = new ServiceCollection();
        Startup startup = new Startup();
        startup.ConfigureServices(services);
        IServiceProvider serviceProvider = services.BuildServiceProvider();
        var service = serviceProvider.GetService<IRestApp>();
        service.GetSomething<Result>("http://test.com").GetAwaiter().GetResult();
    }
}