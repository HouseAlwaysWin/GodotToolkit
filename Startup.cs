using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GodotToolkit.Data;
using GodotToolkit.Services;
using MudBlazor.Services;

namespace GodotToolkit;

public static class Startup
{
    public static IServiceProvider? Services { get; private set; }

    public static void Init()
    {
        var host = Host.CreateDefaultBuilder()
                       .ConfigureServices(WireupServices)
                       .Build();
        Services = host.Services;
    }

    private static void WireupServices(IServiceCollection services)
    {
        services.AddWindowsFormsBlazorWebView();
        services.AddSingleton<WeatherForecastService>();
        services.AddSingleton<IConfigServices, ConfigServices>();
        services.AddMudServices();

#if DEBUG
        services.AddBlazorWebViewDeveloperTools();
#endif
    }
}
